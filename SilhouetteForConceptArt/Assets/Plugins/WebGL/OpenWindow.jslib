var OpenWindowPlugin = {
    openWindow: function(link)
    {
        var url = Pointer_stringify(link);
        // window.open(url);

        const a = document.createElement("a");
        a.href = url;
        a.download = "SilhouetteForConceptArt.jpg";
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    }
};
 
mergeInto(LibraryManager.library, OpenWindowPlugin);