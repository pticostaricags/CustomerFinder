$(document).ready(function(){
    

    $('#main-navbar > li').click(function(){
        $('li').removeClass("item-active");
        $(this).addClass("item-active");
    });

	$('#nav').onePageNav({
	    begin: function() {
	    },
	    end: function() {
	    }
	  });

	$("#view-more").on('click',function(){
		console.log($("#service").offset().top);
		$('html, body').animate({
         scrollTop: $("#service").offset().top
     }, 1500);
	});
 
	$(window).scroll(function(){
		var barra = $(window).scrollTop();
		var posicion = barra * 0.40;
 
		$('body').css({
			'background-position': '0 ' + posicion + 'px'
		});
	});

	$('.system-features').mouseover(function(){
		var thisFeature = $(this);
	    $('#imagen').attr('src', thisFeature.data("feature"));
	});

	$('.system-features').mouseover(function(){
		$('.glyphicon-works').attr('style','color:#1b74ae; font-size:35px;');
		var thisIconWork = $(this);
		$('.glyphicon', thisIconWork).attr('style','color:#62b6ec; transform: scale(1.5);');
	});

	$('.testimonial-style3').owlCarousel({
	      autoPlay: 3000,
	 	  dots: true,
	      items : 3,
	      itemsDesktop : [1199,3],
	      itemsDesktopSmall : [979,3]
	  });



});
