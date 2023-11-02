function getElementArray(elementName, className) {

    var list = document.getElementsByTagName(elementName);
    var value = "";
    var myList = [];
    for (i = 0; i < list.length; i++) {
        n = list[i].className.search(className);

        if (n >= 0) {
            myList.push(list[i]);
        }

    }
    return myList;
}



function waitAndMove(redirectPgName, delayTime) {
    setTimeout(function () { leaveMe(redirectPgName) }, delayTime);
}


// Redirect to requested url   
function leaveMe(redirectPgName) {
    window.location = redirectPgName;
}