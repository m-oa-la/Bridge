function helloWorld() {
    /*
    Logs hello world to the console.
    */
    console.log("Hello World!");
}

function findSortingIndices(arr) {
    /*
    Finds the indices that sorts an array.

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
    for (var i in arrWithIndex) {
        arr.push(arrWithIndex[i][0]);
        indices.push(arrWithIndex[i][1]);
    }

    return indices;
}