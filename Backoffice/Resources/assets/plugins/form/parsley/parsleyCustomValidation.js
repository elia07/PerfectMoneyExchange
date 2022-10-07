window.ParsleyConfig = {
	errorsWrapper: '<ul class="parsley-errors-list"></ul>',
    validators: {
	  nationalID: {
        fn: function (value) {
			var meli_code;
			meli_code=value;
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
      validColor: {
          fn: function (value) {
              var patt = /^#[0-9a-f]{6}$/;
              return patt.test(value.toLowerCase());
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
	  nationalID: 'This NationalID is invalid',
      phone: 'This value should be a valid phone number',
	  keywordselect:'You must select atleast one keyword',
	  fileselect: 'You must select a valid file',
      validcolor:'Invalid Color'
    },
    fr: {
	  nationalID: 'کد ملی معتبر نیست',
      phone: 'این فیلد یک شماره تلفن معتبر را می پذیرد',
	  keywordselect:'باید حداقل یک کلمه کلیدی را انتخاب کنید',
	  fileselect: 'باید یک فایل معتبر انتخاب کنید',
	  validcolor: 'مقدار به عنوان یک رنگ معتبر شناسایی نشد'
    }
  }
  };