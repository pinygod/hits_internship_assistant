

window.onload = function () {
    let headerHeight = $("header").height();
    let bodyHeight = $("body").height();
    let footerHeight = $("footer").height();

    $("#wrapper").height(bodyHeight - headerHeight - footerHeight);
    
}

