var OpenWindowPlugin = {
    openWindow: function(link)
    {
        var url = Pointer_stringify(link);
        window.open(url);
    }
};
 
mergeInto(LibraryManager.library, OpenWindowPlugin);