function datetimehelper_stringToDate(str) {

    var tempStrs = str.split(" ");

    var dateStrs = tempStrs[0].split("-");

    var year = parseInt(dateStrs[0], 10);

    var month = parseInt(dateStrs[1], 10) - 1;

    var day = parseInt(dateStrs[2], 10);

    var timeStrs = tempStrs[1].split(":");

    var hour = parseInt(timeStrs[0], 10);

    var minute = parseInt(timeStrs[1], 10);

    var second = parseInt(timeStrs[2], 10);

    var date = new Date(year, month, day, hour, minute, second);

    return date;
}

function datetimehelper_getSeconds(date) {
    return parseInt(date.getTime() / 1000);
}

function datetimehelper_getSecondsFromNow(dateStr) {

    var d1 = datetimehelper_stringToDate(dateStr);
    var d2 = new Date();

    var s1 = datetimehelper_getSeconds(d1);
    var s2 = datetimehelper_getSeconds(d2);

    var s3 = s1 - s2;

    return s3;    
}