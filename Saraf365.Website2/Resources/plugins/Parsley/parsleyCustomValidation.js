String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};
window.ParsleyConfig = {
	errorsWrapper: '<ul class="parsley-errors"></ul>',
    validators: {
        usernameValidate: {
            fn: function (value) {
                var invalidChars = ['\'', '"', '/', '!', '|', '&', '=', "ا", "ب", "پ", "ت", "ث", "ج", "چ", "ح", "خ", "د", "ذ", "ر", "ز", "ژ", "س", "ش", "ص", "ض", "ط", "ظ", "ع", "غ", "ف", "ق", "ک", "گ", "ل", "م", "ن", "و", "ه", "ی"];
                var hasinValidChar = false;
                for (var i = 0; i < invalidChars.length; i++) {
                    hasinValidChar = value.indexOf(invalidChars[i]) >= 0;
                    if (hasinValidChar) {
                        break;
                    }
                }
                return (!hasinValidChar);
            },
            priority: 32
        },
        justPersianChars: {
            fn: function (value) {
	            return persianRex.letter.test(value);
	        },
	        priority: 32
	    },
	    strongPassword: {
	        fn: function (value) {
	            /*var invalidChars = ['~', '`', '!', '%', '^', '&', '*', '(', ')', '{', '}', '[', ']', '\\', '/'];
	            for (var i = 0; i < invalidChars.length; i++)
	            {
	                alert(invalidChars[i] + " | " + value.indexOf(invalidChars[i]));
	                if (value.indexOf(invalidChars[i]) >= 0) {
	                    return false;
	                }
	            }*/
	            value = value.replaceAll("@", "");
	            value = value.replaceAll("#", "");
	            value = value.replace(/\$/g, '');
	            if (value.length >= 6) {

	                return /^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$/.test(value);
	            }
	            return false;
	        },
	        priority: 32
	    },
	    irCellphone: {
	        fn: function (value) {
	            if (value.length == 11 && value.substring(0, 2) == "09")
	            {
	                return true;
	            }
	            return false;

	            
	        },
	        priority: 32
	    },
	    peIrCellphone: {
	        fn: function (value) {

	            value = value.replaceAll("۰", "0");
	            value = value.replaceAll("۱", "1");
	            value = value.replaceAll("۲", "2");
	            value = value.replaceAll("۳", "3");
	            value = value.replaceAll("۴", "4");
	            value = value.replaceAll("۵", "5");
	            value = value.replaceAll("۶", "6");
	            value = value.replaceAll("۷", "7");
	            value = value.replaceAll("۸", "8");
	            value = value.replaceAll("۹", "9");

	            value = value.replaceAll("٠", "0");
	            value = value.replaceAll("١", "1");
	            value = value.replaceAll("٢", "2");
	            value = value.replaceAll("٣", "3");
	            value = value.replaceAll("٤", "4");
	            value = value.replaceAll("٥", "5");
	            value = value.replaceAll("٦", "6");
	            value = value.replaceAll("٧", "7");
	            value = value.replaceAll("٨", "8");
	            value = value.replaceAll("٩", "9");
                

	            if (value.length == 11 && value.substring(0, 2) == "09") {
	                return true;
	            }
	            return false;


	        },
	        priority: 32
	    },
	    peDigits: {
	        fn: function (value) {
	            var str = String(value);
	            str = str.replaceAll("۰", "0");
	            str = str.replaceAll("۱", "1");
	            str = str.replaceAll("۲", "2");
	            str = str.replaceAll("۳", "3");
	            str = str.replaceAll("۴", "4");
	            str = str.replaceAll("۵", "5");
	            str = str.replaceAll("۶", "6");
	            str = str.replaceAll("۷", "7");
	            str = str.replaceAll("۸", "8");
	            str = str.replaceAll("۹", "9");

	            str = str.replaceAll("٠", "0");
	            str = str.replaceAll("١", "1");
	            str = str.replaceAll("٢", "2");
	            str = str.replaceAll("٣", "3");
	            str = str.replaceAll("٤", "4");
	            str = str.replaceAll("٥", "5");
	            str = str.replaceAll("٦", "6");
	            str = str.replaceAll("٧", "7");
	            str = str.replaceAll("٨", "8");
	            str = str.replaceAll("٩", "9");

	            return /^\d+$/.test(str);
	        },
	        priority: 32
	    },
	  nationalID: {
	      fn: function (value) {
	          var str = String(value);
	          str = str.replaceAll("۰", "0");
	          str = str.replaceAll("۱", "1");
	          str = str.replaceAll("۲", "2");
	          str = str.replaceAll("۳", "3");
	          str = str.replaceAll("۴", "4");
	          str = str.replaceAll("۵", "5");
	          str = str.replaceAll("۶", "6");
	          str = str.replaceAll("۷", "7");
	          str = str.replaceAll("۸", "8");
	          str = str.replaceAll("۹", "9");

	          str = str.replaceAll("٠", "0");
	          str = str.replaceAll("١", "1");
	          str = str.replaceAll("٢", "2");
	          str = str.replaceAll("٣", "3");
	          str = str.replaceAll("٤", "4");
	          str = str.replaceAll("٥", "5");
	          str = str.replaceAll("٦", "6");
	          str = str.replaceAll("٧", "7");
	          str = str.replaceAll("٨", "8");
	          str = str.replaceAll("٩", "9");


			var meli_code;
			meli_code = str;

			if (meli_code.length == 10)
			{
			if(meli_code=='1111111111' ||
			meli_code=='0000000000' ||
			meli_code=='2222222222' ||
			meli_code=='3333333333' ||
			meli_code=='4444444444' ||
			meli_code=='5555555555' ||
			meli_code=='6666666666' ||
			meli_code=='7777777777' ||
			meli_code=='8888888888' ||
			meli_code=='9999999999' )
			{
			return false;
			}
			c = parseInt(meli_code.charAt(9));
			n = parseInt(meli_code.charAt(0))*10 +
			parseInt(meli_code.charAt(1))*9 +
			parseInt(meli_code.charAt(2))*8 +
			parseInt(meli_code.charAt(3))*7 +
			parseInt(meli_code.charAt(4))*6 +
			parseInt(meli_code.charAt(5))*5 +
			parseInt(meli_code.charAt(6))*4 +
			parseInt(meli_code.charAt(7))*3 +
			parseInt(meli_code.charAt(8))*2;
			r = n - parseInt(n/11)*11;
			if ((r == 0 && r == c) || (r == 1 && c == 1) || (r > 1 && c == 11 - r))
			{
			return true;
			}
			else
			{
			return false;
			}
			}
			else
			{
			return false;
			}
        },
        priority: 32
      },
      phone: {
        fn: function (value) {
          var patt=/^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}$/;
		  return patt.test(value);
        },
        priority: 32
      },
	  keywordselect: {
        fn: function (value) {
		  if($(".keywordTag").length==0)
			return false;
		  return true;
        },
        priority: 32
      },
	  fileselect: {
        fn: function (inputTarget) {			
			if($("#"+inputTarget).val().length==0)
				return false;
			return true;
        },
        priority: 32
      }
    },
	i18n: {
    en: {
      strongpassword:"Password must ontain alphabet and numbers",
      ircellphone: "This field must be a valid cellphone",
      peircellphone: "This field must be a valid cellphone",
      pedigits: 'This field must Contains only digits',
	  nationalid: 'This NationalID is invalid',
      phone: 'This value should be a valid phone number',
	  keywordselect:'You must select atleast one keyword',
	  fileselect:'You must select a valid file',
        justpersianchars: 'Just persian chars are allowed',
        usernameValidate:'username has invalid characters'
    },
    fa: {
      strongpassword: "رمزعبور می بایست شامل عدد و حرف باشد",
      ircellphone: "شماره موبایل معتبر نیست",
      peircellphone: "شماره موبایل معتبر نیست",
      pedigits: 'این فیلد فقط باید شامل اعداد باشد.',
	  nationalid: 'کد ملی معتبر نیست',
      phone: 'این فیلد یک شماره تلفن معتبر را می پذیرد',
	  keywordselect:'باید حداقل یک کلمه کلیدی را انتخاب کنید',
	  fileselect: 'باید یک فایل معتبر انتخاب کنید',
        justpersianchars: 'فقط حروف فارسی مجاز است',
        usernameValidate:'نام کاربری دارای حروف غیر مجاز می باشد ، حروف فارسی مجاز نیست'
    }
  }
  };