
//Below codes solve 'includes' method not working in IE
String.prototype.includes = function (str) {
    var returnValue = false;

    if (this.indexOf(str) !== -1) {
        returnValue = true;
    }

    return returnValue;
};

function findSortingIndices(arr) {
    /*
    Finds the indices that sorts an 1D array of numbers.
    :arg arr: an array of numbers, the data
    :return: an array of numbers, the sorting indices of the data
    */
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

function filterArray(arr, toRemove) {
    /*
    Removes the elements that are contained in one array from the other.
    :arg arr: list, the array
    :arg toRemove: list, the elements to be removed
    */
    arr = arr.filter(function (el) {
        return toRemove.indexOf(el) < 0;
    });

    return arr;
}