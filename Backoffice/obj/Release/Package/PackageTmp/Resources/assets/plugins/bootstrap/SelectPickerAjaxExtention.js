(function ( $ ) {
	var Path;
	var TagsContainer;
	$.fn.selectPickerAjaxExtention = function(path,tagsContainer,parsleyGroup,parsleyContainer) {
		Path=path;
		TagsContainer=tagsContainer;
		ParsleyGroup=parsleyGroup;
		ParsleyContainer=parsleyContainer
		var firstChild=$(this).next().children()[0];
		var secondChild=$(this).next().children()[1];
		var selectContainer=$(this);
		
		 $(firstChild).children(".filter-option").html('کلمه کلیدی خود را جستجو و یا تایپ کنید');
		
		var secondchildChildren=$(secondChild).children()[0];
		var updateContainer=$(secondChild).children()[1]
		
		var intputText=$(secondchildChildren).children()[0];
		$(intputText).bind("change paste keyup", function() {
			$(updateContainer).html('');
		   if($(this).val().length>=3)
			{
				 var args = {
					"sample": $(this).val()
				};
				$.ajax({
				url: Path, data: JSON.stringify(args), type: "POST", dataType: 'json',contentType: "application/json; charset=utf-8",
				success: function (result) {
					var htmlStr1="";
					for(var keyword in result.keywords)
					    htmlStr1 += '<li rel="' + keyword + '" class="liveSearchResult" keyword="' + result.keywords[keyword] + '"><a tabindex="0" class="" style=""><span class="text">' + result.keywords[keyword] + '</span><i class="glyphicon glyphicon-ok icon-ok check-mark"></i></a></li>';
					$(updateContainer).html(htmlStr1);
					$('.liveSearchResult').click(function () {
					    $(TagsContainer).append(TagGenerator($(this).attr("keyword")));
					});
				},
				statusCode: { 404: ajaxError_404, 505: ajaxError_505 }, error: ajaxError_error
			});
			}
		});
		
		$($(secondchildChildren).children()[0]).keydown(function(event){
			var keycode = (event.keyCode ? event.keyCode : event.which);
			if(keycode == '13'){
			    $(TagsContainer).append(TagGenerator($(this).val()));
				$(ParsleyContainer).parsley().validate(ParsleyGroup);
			}
		});
	};
	function TagGenerator(value)
	{
	    return '<span class="tag label label-success keywordTag" style="font-size:18px;">' +value.trim() + '<span onclick="$(this).parent().remove();$(ParsleyContainer).parsley().validate(ParsleyGroup);" class="glyphicon glyphicon-remove tagInputRemove"></span></span>';
	}
}( jQuery ));