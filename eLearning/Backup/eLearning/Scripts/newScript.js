
function ShowMyModalPopup(id)
    var modal = $find('ModalPopupExtender1');
    modal.show();
    document.getElementById('myIframe').src = id;
}
function HideModalPopup() {
    var modal = $find('ModalPopupExtender1');
    modal.hide();
    document.getElementById('myIframe').src = "";
}
    