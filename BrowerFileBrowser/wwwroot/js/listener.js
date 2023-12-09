function addOutsideClickListener(dotNetReference) {
    document.addEventListener('click', function (event) {
        var popover = document.querySelector('.mud-popover-open');
        if (popover) {
            var rect = popover.getBoundingClientRect();
            var isClickInsidePopover = event.clientX >= rect.left &&
                event.clientX <= rect.right &&
                event.clientY >= rect.top &&
                event.clientY <= rect.bottom;

            if (!isClickInsidePopover) {
                dotNetReference.invokeMethodAsync('ClosePopover');
            }
        }
    });
}
