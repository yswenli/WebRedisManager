//获取url中"?"符后的字串
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串   
    var theRequest = new Object();
    if (url.indexOf("?") !== -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}

function HttpGet(url, data, success, error) {
    const headers = new Headers();
    headers.append('Content-Type', 'application/x-www-form-urlencoded');
    fetch(url, {
        method: 'GET',
        headers: headers,
        cache: 'no-store'
    })
        .then(response => response.json())
        .then((result) => {
            console.log("Success:", result);
            success(result);
        })
        .catch(e => {
            console.log('Error:', e);
            if (error) error(e);
        });
}


function HttpPost(url, data, success, error) {
    const headers = new Headers();
    headers.append('Content-Type', 'application/x-www-form-urlencoded');
    fetch(url, {
        method: 'POST',
        headers: headers,
        body: data
    })
        .then(response => response.json())
        .then((result) => {
            console.log("Success:", result);
            success(result);
        })
        .catch(e => {
            console.log('Error:', e);
            if (error) error(e);
        });
}