using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MABridge.OpenXml
{
    public class OpenXmlWordHelper
    {
        public static void FillForm(Dictionary<string, string> fieldValues, Stream stream)
        {
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(stream, true))
            {
                var doc = wordprocessingDocument.MainDocumentPart.Document;
                IEnumerable<FormFieldData> fields = doc.Descendants<FormFieldData>();
                foreach (var field in fields)
                {
                    var ffName = field.Descendants<FormFieldName>().FirstOrDefault();
                    var bookmarkName = ffName.Val.Value;

                    if(fieldValues.Keys.Contains(bookmarkName)){

                        var ffData = ffName.Parent;
                        var fldChar = ffData.Parent;

                        var runPrs = fldChar.Parent.ChildElements.FirstOrDefault(x => x.LocalName == "rPr");


                        var para = ffName.Ancestors<Paragraph>().FirstOrDefault();
                        para.ChildElements.ToList().ForEach(x =>
                        {
                            if (x.LocalName == "r" || x.LocalName == "bookmarkEnd")
                            {
                                x.Remove();
                            }
                        });


                        var r = new Run();
                        if (runPrs != null)
                        {
                            r.RunProperties = new RunProperties(runPrs.CloneNode(true));
                        }

                        var bookmarkStart = doc.Descendants<BookmarkStart>().FirstOrDefault(x => x.Name == bookmarkName);
                        bookmarkStart.Remove();

                        var text = fieldValues[bookmarkName];
                        if (string.IsNullOrEmpty(text)) { text = string.Empty; }
                        var result = Regex.Split(text, "\r\n|\r|\n");
                        

                        r.AppendChild(new Text(result[0]));
                        para.AppendChild<Run>(r);

                        if (result.Count() > 1)
                        {
                            var pre = para.First().Parent;
                            result.Skip(1).ToList().Select(txt =>
                            {
                                var p = para.CloneNode(true);
                                var textNode = p.Descendants<Text>().FirstOrDefault();
                                textNode.Text = txt;
                                return p;
                            }).ToList().ForEach(p =>
                            {
                                pre.InsertAfterSelf(p);
                                pre = p;
                            });
                        }
                    }
                }
            }
        }
    }
}
