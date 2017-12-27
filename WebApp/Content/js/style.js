$(document).ready(function () {

	//hadle back to top
	$(window).on("scroll",function(){
		if ($(this).scrollTop()>20) {
			$(".scroll-to-top").fadeIn();
		}else{
			$(".scroll-to-top").fadeOut();
		}
	});
	$(".scroll-to-top").on("click",function(){
		$(".scroll-to-top").tooltip('hide');
		$('body,html').animate({
			scrollTop:0
		},1000);
		return false;
	});
	$(".scroll-to-top").tooltip('show');
    //end hadle back to top

    //nasty work from localstorage
	if (localStorage.getItem("megaModuleId")) {
	    var moduleId = localStorage.getItem("megaModuleId");
	    var markup = $("#" + moduleId).html();
	    var title = $("#" + moduleId).closest('.dropdown-menu').prev('.dropdown-toggle').text();
	    $("header .navbar-nav>li.sibling-menu .sub-scroll").empty();
	    $("header .navbar-nav>li.sibling-menu>a").html(title);
	    var menuItemId = localStorage.getItem("megaMenuItem");

	    window.megaModuleIds.push({
	        megaModuleId: moduleId,
	        megaMenuItem: menuItemId
	    });
	    $("header .navbar-nav>li.sibling-item>a").empty().html($("#" + menuItemId+ " a").text());
	    $("header .navbar-nav>li.sibling-menu .sub-scroll").append('<div><ul>' + markup + '</ul></div>');
	    $('header .navbar-nav>li.sibling-menu .dropdown-menu .sub-scroll div>ul>li>ul>li a').on('click', function () {
	        $("header .navbar-nav>li.sibling-item>a").empty().html($(this).text());
	        itemId = $(this).closest("li").attr("id");
	        window.megaModuleIds.push({
	            megaModuleId: moduleId,
	            megaMenuItem: itemId
	        });
	        localStorage.setItem("megaMenuItem", itemId);


	    });
	   
	}
	if (localStorage.getItem("pinMenu")) {
	    var bool = localStorage.getItem("pinMenu");
	    if (bool === "true") {
	        $('aside , .toggle-menu , body').addClass('opened');
	    }
	    else {
	        $('aside , .toggle-menu , body').removeClass('opened');
	    }
	}
    //end nasty

	$('.toggle-menu').click(function () {
        $('aside').toggleClass('opened');
        $(this).toggleClass('opened');
        if ($('aside').hasClass('opened') && $(this).hasClass('opened')) {
            localStorage.setItem("pinMenu", true);
        } else {
            localStorage.setItem("pinMenu", false);
        }
    });

    $('aside').hover(function () {
        $('body').addClass('opened')
    });

    $('aside').mouseleave(function () {
        if (!$('.toggle-menu').hasClass('opened')) {
            $('body').removeClass('opened')
        }
    });

    $("aside .scroll").mCustomScrollbar({
        theme: "minimal-dark"
    });

    $("header li.list").mCustomScrollbar({
        theme: "minimal-dark"
    });

    $("header .scroll-mega").mCustomScrollbar({
        theme: "minimal-dark"
    });

    $('header #mega>.dropdown-toggle').click(function(){
        $('#mega').toggleClass('open');
        
    });
    $('header #mega>.dropdown-menu .mega-cont a').on('click', function () {
        
        var markup = $(this).closest('.dropdown-menu').find('.mega-cont').html();
        var title = $(this).closest('.dropdown-menu').prev('.dropdown-toggle').text();
        var markupId = $(this).closest('.dropdown-menu').find('.mega-cont').attr('id');
        var itemId = $(this).closest("li").attr("id");
        localStorage.setItem("megaModuleId", markupId);

        window.megaModuleIds.push({
            megaModuleId: markupId,
            megaMenuItem: itemId
        });
       
        localStorage.setItem("megaMenuItem", itemId);
       
        $("header .navbar-nav>li.sibling-menu .sub-scroll").empty();
        $("header .navbar-nav>li.sibling-menu>a").html(title);
        $("header .navbar-nav>li.sibling-item>a").empty().html($(this).text());
        $("header .navbar-nav>li.sibling-menu .sub-scroll").append('<div><ul>' + markup + '</ul></div>');


        $('header .navbar-nav>li.sibling-menu .dropdown-menu .sub-scroll div>ul>li>ul>li a').on('click', function () {
            $("header .navbar-nav>li.sibling-item>a").empty().html($(this).text());
            itemId = $(this).closest("li").attr("id");
            localStorage.setItem("megaMenuItem", itemId);
            window.megaModuleIds.push({
                megaModuleId: markupId,
                megaMenuItem: itemId
            });
            
        });
        $('header .navbar-nav>li.mega.open').removeClass('open');
        $("header .navbar-nav>li.sibling-menu .sub-scroll>div").mCustomScrollbar({
            theme: "minimal-dark"
        });
    });
    
    $('.page-content-wrapper , aside , footer').click(function () {
    	$('#mega').removeClass('open')
    });

    window.addEventListener("popstate", function (event) {
        var currentmegaModuleId = megaModuleIds[megaModuleIds.length - 2]
        
        if (megaModuleIds.length > 1) {
           
        }
        
        if (currentmegaModuleId) {
            megaModuleIds.pop();
                var moduleId = currentmegaModuleId.megaModuleId;
                var markup = $("#" + moduleId).html();
                
                var title = $("#" + moduleId).closest('.dropdown-menu').prev('.dropdown-toggle').text();
                $("header .navbar-nav>li.sibling-menu .sub-scroll").empty();
                $("header .navbar-nav>li.sibling-menu>a").html(title);
                var menuItemId = currentmegaModuleId.megaMenuItem;
            

                $("header .navbar-nav>li.sibling-item>a").empty().html($("#" + menuItemId + " a").text());
                $("header .navbar-nav>li.sibling-menu .sub-scroll").append('<div><ul>' + markup + '</ul></div>');
                $('header .navbar-nav>li.sibling-menu .dropdown-menu .sub-scroll div>ul>li>ul>li a').on('click', function () {
                    $("header .navbar-nav>li.sibling-item>a").empty().html($(this).text());
                    itemId = $(this).closest("li").attr("id");
                    localStorage.setItem("megaMenuItem", itemId);
                    window.megaModuleIds.push({
                        megaModuleId: currentmegaModuleId.megaModuleId,
                        megaMenuItem: itemId
                    });
                });

                //$("#" + menuItemId + ">a").click();
            }
        
    });


});