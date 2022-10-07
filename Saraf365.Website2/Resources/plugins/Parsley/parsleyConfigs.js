window.ParsleyConfig = {
    errorsContainer: function (el) {
        return el.$element.closest('.form-group');
    },
    classHandler: function (el) {
        return el.$element.closest(".form-group");
    }
};
