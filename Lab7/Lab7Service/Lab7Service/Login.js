const LOGIN_BASE = "localhost:59510/Login.svc/";

function $(id) {
    return document.getElementById(id);
}

function auth() {
    //Grab username and password from user entry
    user = $('username').textContent;
    pass = $('password').textContent;

    //Use REST Service
    var url = LOGIN_BASE + user.toString() + "/" + pass.toString();

    var isRegistered = httpGet(url);

    if (isRegistered === 'true') {
        //Return welcome msg
        window.location.href = "Welcome.html";
        $('usr-wel').innerHTML = "Welcome " + user;

    } else if(isRegistered === 'false'){
        //User needs to register
        $('login-err').innerHTML = "You haven't registered yet. Please register before logging in.";
    } else {
        $('login-err').innerHTML = "A URL Error Occurred.";
    }    
}

function httpGet(url) {
    var xmlHttp = null;

    xmlHttp = new XMLHttpRequest();
    xmlHttp.open("POST", url, true);    
    xmlHttp.send(null);
    return xmlHttp.responseText;

}