//http://viget.com/inspire/extending-paul-irishs-comprehensive-dom-ready-execution

var SITENAME = {};

var UTIL = {
    exec: function (controller, action) {
        var ns = SITENAME,
            action = (action === undefined) ? "init" : action;

        if (controller !== "" && ns[controller] && typeof ns[controller][action] == "function") {
            ns[controller][action]();
        }
    },

    init: function () {
        var body = document.body,
            controller = body.getAttribute("data-controller"),
            action = body.getAttribute("data-action");

        UTIL.exec("common");
        UTIL.exec(controller);
        UTIL.exec(controller, action);
    },

    // http://stackoverflow.com/questions/9812587/referencing-one-java-script-library-from-another

    loadScript: function (file_name) {
        if (document.readyState === 'loading') { // Chrome
            document.write('<script src="' + file_name.replace(/"/g, '&quot;') + '"></script>');
            return;
        }
        var newScript = document.createElement('script');
        var scripts = document.getElementsByTagName('script');

        // Reference to the latest (this) <script> tag in the document
        scripts = scripts[scripts.length - 1];

        // Set target
        newScript.src = file_name;

        // Clean-up code:
        newScript.onload = newScript.onerror = function () {
            this.parentNode.removeChild(this);
        };

        // Insert script in the document, to load it.
        scripts.parentNode.insertBefore(newScript, scripts);
    }

};


(function ($) {
    $(document).ready(UTIL.init);
})(jQuery);