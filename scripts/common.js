function openPopup(url, w = 400, h = 400) {
    var popup = window.open(url, "popup", `width=${w},height=${h}`);
    popup.focus();
}
