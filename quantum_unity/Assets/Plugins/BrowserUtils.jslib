var WebGLFunctions = {
    
    ToggleLoginIframe: function (show) {
        var iframe = document.getElementById("loginIframe");
        if (iframe) {
            iframe.style.display = show === 1 ? "block" : "none";
        } else {
            console.warn("Login iframe element not found in the DOM");
        }
    },
    
    IsMobileBrowser: function () {
        return (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent));
    }
};

mergeInto(LibraryManager.library, WebGLFunctions);