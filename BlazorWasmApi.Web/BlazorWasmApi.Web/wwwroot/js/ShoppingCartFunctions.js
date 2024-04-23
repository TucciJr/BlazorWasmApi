﻿function MakeUpdateQtyButtonVisible(id, visible) {
    const updateQtyButton = document.querySelector(`button[data-item='${id}']`);

    if (visible == true) {
        updateQtyButton.style.display = "inline-block";
    }
    else {
        updateQtyButton.style.display = "none";
    }
}