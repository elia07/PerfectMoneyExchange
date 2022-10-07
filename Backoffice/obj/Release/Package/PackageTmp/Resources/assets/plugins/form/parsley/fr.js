// ParsleyConfig definition if not already set
window.ParsleyConfig = window.ParsleyConfig || {};
window.ParsleyConfig.i18n = window.ParsleyConfig.i18n || {};

// Define then the messages
window.ParsleyConfig.i18n.fr = $.extend(window.ParsleyConfig.i18n.fr || {}, {
  defaultMessage: "مقدار این فیلد نامعتبر است",
  type: {
    email:        "مقدار این فیلد باید یک آدرس پست الکترونیک معتبر باشد.",
    url:          "مقدار این فیلد باید یک آدرس اینترنتی معتبر باشد",
    number:       "مقدار این فیلد باید یک  عدد معتبر باشد.",
    integer:      "مقدار این فیلد باید یک عدد صحیح معبر باشد.",
    digits:       "مقدار این فیلد باید فقط شامل ارقام باشد.",
    alphanum:     "مقدار این فیلد باید شامل عدد و حرف باشد."
  },
  notblank:       "این فیلد حتما باید مقدار داشته باشد.",
  required:       "پر کردن این فیلد الزامی است.",
  pattern:        "مقدار این فیلد نا معتبر است.",
  min:            "مقدار این فیلد باید بزرگتر مساوی %s باشد.",
  max:            "مقدار این فیلد باید کوچکتر مساوی %s باشد.",
  range:          "مقدار این فیلد باید بین %s و %s باشد.",
  minlength:      "مقدار این فیلد باید حداقل %s کاراکتر داشته باشد.",
  maxlength:      "حداکثر تعداد کاراکتر برای این فیلد %s است.",
  length:         "مقدار این فیلد باید بین %s و %s باشد.",
  mincheck:       "می بایست حداقل %s تعداد گزینه را انتخاب کنید.",
  maxcheck:       "حداکثر تعداد انتخاب %s است.",
  check:          "باید %s از %s گزینه را اینتخاب کنید.",
  equalto:        "مقدار این فیلد باید تکرار شود."
});

// If file is loaded after Parsley main file, auto-load locale
if ('undefined' !== typeof window.ParsleyValidator)
  window.ParsleyValidator.addCatalog('fr', window.ParsleyConfig.i18n.fr, true);