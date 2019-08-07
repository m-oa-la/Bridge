String.prototype.includes = function (str) {//If not supported, then define the method
    return this.indexOf(str) != -1;
};

Array.prototype.includes = function (str) {//If not supported, then define the method
    return this.indexOf(str) !== -1;
};


String.prototype.startsWith = function (searchString, position) {
    position = position || 0;
    return this.indexOf(searchString, position) === position;
};


if (!Array.prototype.fill) {
    Object.defineProperty(Array.prototype, 'fill', {
        value: function (value) {

            // Steps 1-2.
            if (this == null) {
                throw new TypeError('this is null or not defined');
            }

            var O = Object(this);

            // Steps 3-5.
            var len = O.length >>> 0;

            // Steps 6-7.
            var start = arguments[1];
            var relativeStart = start >> 0;

            // Step 8.
            var k = relativeStart < 0 ?
                Math.max(len + relativeStart, 0) :
                Math.min(relativeStart, len);

            // Steps 9-10.
            var end = arguments[2];
            var relativeEnd = end === undefined ?
                len : end >> 0;

            // Step 11.
            var final = relativeEnd < 0 ?
                Math.max(len + relativeEnd, 0) :
                Math.min(relativeEnd, len);

            // Step 12.
            while (k < final) {
                O[k] = value;
                k++;
            }

            // Step 13.
            return O;
        }
    });
}

function findSortingIndices(arr) {
    /* Finds the indices that sorts an 1D array of numbers.
     * arg arr: an array of numbers, the data
     * return: an array of numbers, the sorting indices of the data */
    var arrWithIndex = [];
    for (var i in arr) {
        arrWithIndex.push([arr[i], Number(i)]);
    }
    // Overloading sort function
    arrWithIndex.sort(function (left, right) {
        return left[0] < right[0] ? -1 : 1;
    });
    var indices = [];
    arr = [];
    for (var ii in arrWithIndex) {
        arr.push(arrWithIndex[ii][0]);
        indices.push(arrWithIndex[ii][1]);
    }
    return indices;
}

function getObject(obj, key, value) {
    /*
    Returns every key-value pair from a nested object.
    :arg obj: object, the object
    :arg key: int, the key
    :arg value: -, the value
    */
    var object = [];
    for (var i in obj) {
        if (!obj.hasOwnProperty(i)) {
            continue;
        } else {
            if (typeof obj[i] === 'object') {
                object = object.concat(getObject(obj[i], key, value));
            } else if (i === key && obj[key] === value) {
                object.push(obj);
                break;
            }
        }
    }

    return object;
}

function hasDuplicates(arr) {
    /*
    Checks if an array has duplicate elements.
    :arg arr: array
    */
    return (new Set(arr)).size !== arr.length;
}

function getTodayDate() {
    /*
    Returns the current date and time.
    :return: string, the formatted date
    */
    var currentdate = new Date();
    var m = currentdate.getMonth() + 1;
    var dt = currentdate.getFullYear() + "." + m + "."
        + currentdate.getDate() + " "
        + currentdate.getHours() + ":"
        + currentdate.getMinutes() + ":"
        + currentdate.getSeconds();
    return dt;
}

function arrayApplyFilter(arr, filter) {
    /* Removes the elements of a filter from an array.
     * arg arr: array
     * arg filter: array */
    arr = arr.filter(function (el) {
        return filter.indexOf(el) < 0;
    });
    return arr;
}

function arrayToLower(arr) {
    /* Makes the string elements of an array lower case.
     * arg arr: array */
    var elem = null;
    for (var i = 0; i < arr.length; i++) {
        elem = arr[i];
        if (typeof elem == "string") {
            arr[i] = arr[i].toLowerCase();
        }
    }
    return arr;
}

function arrayToUpper(arr) {
    /* Makes the string elements of an array upper case.
     * arg arr: array */
    var elem = null;
    for (var i = 0; i < arr.length; i++) {
        elem = arr[i];
        if (typeof elem == "string") {
            arr[i] = arr[i].toUpperCase();
        }
    }
    return arr;
}

function arrayHasElement(arr, el) {
    return !(arr.indexOf(el) < 0);
}

function arrayRemoveEmpties(arr) {
    /* Removes empty strings from an array.
     * arg arr: array */
    arr = arr.filter(function (el) {
        return el != "";
    });
    return arr;
}

function arrayRemoveDuplicates(arr) {
    /* Removes duplicate entries from an array.
     * arg arr: list 
     * return: list */
    var uniques = [];
    var elem = null;
    for (var i = 0; i < arr.length; i++) {
        elem = arr[i];
        if (uniques.indexOf(elem) == -1) {
            uniques.push(elem);
        }
    }
    return uniques;
}

function filterArray(arr, toRemove) {
    /* Removes the elements that are contained in one array from the other.
     * arg arr: list
     * arg toRemove: list, the elements to be removed */
    arr = arr.filter(function (el) {
        return toRemove.indexOf(el) < 0;
    });
    return arr;
}

function getObjectAttributeValues(objects, attribute) {
    /* Returns the attribute values from a list of objects.
     * arg objects: list of objects
     * arg attribute: string
     * return: list */
    var list = [];
    var object = null;
    for (var i = 0; i < objects.length; i++) {
        object = objects[i];
        if (object.hasOwnProperty(attribute)) {
            list.push(object[attribute]);
        }
    }
    return list;
}

function getObjectsWithAttribute(objects, attribute, value) {
    /* Returns the objects that have an attribute with a certain value.
     * arg objects: list of objects
     * arg attribute: string
     * arg value: - 
     * return: list of objects */
    var list = [];
    var object = null;
    for (var i = 0; i < objects.length; i++) {
        object = objects[i];
        if (object.hasOwnProperty(attribute)) {
            if (object[attribute] == value) {
                list.push(object);
            }
        }
    }
    return list;
}