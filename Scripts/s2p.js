$( document ).ready(function() {
	 AOS.init({
      disable: 'mobile',
       delay: 200
      
    });

    $(window).scroll(startCounter);
	startCounter();


});

function startCounter() {
		    if ($(window).scrollTop() > 200) {
		        $(window).off("scroll", startCounter);
		        $('.numbers').each(function () {
		            var $this = $(this);
		            jQuery({ Counter: 0 }).animate({ Counter: $this.text() }, {
		                duration: 1000,
		                easing: 'swing',
		                step: function () {
		                    $this.text(Math.ceil(this.Counter));
		                }
		            });
		        });
		    }
		}