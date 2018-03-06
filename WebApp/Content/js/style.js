$(document).ready(function () {

    //hadle back to top
    $(window).on("scroll", function () {
        if ($(this).scrollTop() > 20) {
            $(".scroll-to-top").fadeIn();
        } else {
            $(".scroll-to-top").fadeOut();
        }
    });
    $(".scroll-to-top").on("click", function () {
        $(".scroll-to-top").tooltip('hide');
        $('body,html').animate({
            scrollTop: 0
        }, 1000);
        return false;
    });
    $(".scroll-to-top").tooltip('show');
    //end hadle back to top

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

    $('header #mega>.dropdown-toggle').click(function () {
        $('#mega').toggleClass('open');

    });

    
    //work from localstorage
    // if (localStorage.getItem("megaModuleId")) {
    //     var moduleId = localStorage.getItem("megaModuleId");
    //     var markup = $("#"+ moduleId).html();
       
    //     var title = $("#"+moduleId).prev('h3').text();
    //     // if ($("#"+ moduleId).closest('ul').prev('h3').length == 0) {
    //     //     // title = $("ul[data-parentid='" + moduleId +"']").closest('.dropdown-menu').closest('.dropdown-menu').prev('a').text();
    //     //     // markup =  $("ul[data-parentid='" + moduleId +"']").html();
    //     // }
    //     $("header .navbar-nav>li.sibling-menu .dropdown-menu").empty();
    //     $("header .navbar-nav>li.sibling-menu>a").html(title);
    //     var menuItemId = localStorage.getItem("megaMenuItem");

    //     window.megaModuleIds.push({
    //         megaModuleId: moduleId,
    //         megaMenuItem: menuItemId
    //     });
    //     $("header .navbar-nav>li.sibling-item>a").empty().html($("#" + menuItemId + " a").text());
    //     $("header .navbar-nav>li.sibling-menu .dropdown-menu").append(markup);
    //     $('header .navbar-nav>li.sibling-menu .dropdown-menu .sub-scroll div>ul>li>ul>li a').on('click', function () {
    //         $("header .navbar-nav>li.sibling-item>a").empty().html($(this).text());
    //         itemId = $(this).closest("li").attr("id");
    //         window.megaModuleIds.push({
    //             megaModuleId: moduleId,
    //             megaMenuItem: itemId
    //         });
    //         localStorage.setItem("megaMenuItem", itemId);


    //     });

    // }
    if (localStorage.getItem("pinMenu")) {
        var bool = localStorage.getItem("pinMenu");
        if (bool === "true") {
            $('aside , .toggle-menu , body').addClass('opened');
        }
        else {
            $('aside , .toggle-menu , body').removeClass('opened');
        }
    }
    //end from localstorage


    // $('header #mega>.dropdown-menu .mega-cont a').on('click', function () {

    //     var markup = $(this).closest('ul').html();
    //     var title = $(this).closest('ul').prev('h3').text();
    //     var markupId = $(this).closest('ul').attr('id');
    //     var itemId = $(this).closest("li").attr("id");
    //     localStorage.setItem("megaModuleId", markupId);

    //     if (title.length == 0) {
    //         title = $(this).closest('.dropdown-menu').closest('.dropdown-menu').prev('a').text();
    //     }

    //     // window.megaModuleIds.push({
    //     //     megaModuleId: markupId,
    //     //     megaMenuTitle: title
    //     // });

    //     // localStorage.setItem("megaMenuItem", itemId);

    //     // $("header .navbar-nav>li.sibling-menu>a").html(title);
    //     // $("header .navbar-nav>li.sibling-item>a").empty().html($(this).text());
    //     // $("header .navbar-nav>li.sibling-menu .dropdown-menu").empty().append(markup);


    //     // $('header .navbar-nav>li.sibling-menu .dropdown-menu .sub-scroll div>ul>li>ul>li a').on('click', function () {
    //     //     // $("header .navbar-nav>li.sibling-item>a").empty().html($(this).text());
    //     //     itemId = $(this).closest("li").attr("id");
    //     //     localStorage.setItem("megaMenuItem", itemId);
    //     //     window.megaModuleIds.push({
    //     //         megaModuleId: markupId,
    //     //         megaMenuTitle: title
    //     //     });

    //     // });
    //     $('header .navbar-nav>li.mega.open').removeClass('open');
    //     $("header .navbar-nav>li.sibling-menu .sub-scroll>div").mCustomScrollbar({
    //         theme: "minimal-dark"
    //     });
    // });

    $('.page-content-wrapper , aside , footer, .mega-cont a').click(function () {
        $('#mega').removeClass('open')
    });


    $('.navbar-nav .mega .dropdown-menu .title').each(function(){
        $(this).on('click',function(){
            var title= $(this).find('h4').text();
            var markup= $(this).next('ul').html();

            $('.sibling-menu .dropdown-toggle').empty().html(title);
            $('.sibling-menu .dropdown-menu').empty().html(markup);
            setTimeout(function(){
                $('.sibling-menu').addClass('open');
            },200);
        $('.sibling-menu .dropdown-menu li').each(function(){
            $(this).on('click',function(){
                var title= $(this).text();
                $('.sibling-item a').empty().text(title);
            });
        });
        });
    });

    // window.addEventListener("popstate", function (event) {
    //     var currentmegaModuleId = megaModuleIds[megaModuleIds.length - 2]

    //     if (megaModuleIds.length > 1) {

    //     }

    //     if (currentmegaModuleId) {
    //         megaModuleIds.pop();
    //         var moduleId = currentmegaModuleId.megaModuleId;
    //         var markup = $("#" + moduleId).html();

    //         var title = $("#" + moduleId).closest('.dropdown-menu').prev('.dropdown-toggle').text();
    //         $("header .navbar-nav>li.sibling-menu .sub-scroll").empty();
    //         $("header .navbar-nav>li.sibling-menu>a").html(title);
    //         var menuItemId = currentmegaModuleId.megaMenuItem;


    //         $("header .navbar-nav>li.sibling-item>a").empty().html($("#" + menuItemId + " a").text());
    //         $("header .navbar-nav>li.sibling-menu .sub-scroll").append('<div><ul>' + markup + '</ul></div>');
    //         $('header .navbar-nav>li.sibling-menu .dropdown-menu .sub-scroll div>ul>li>ul>li a').on('click', function () {
    //             $("header .navbar-nav>li.sibling-item>a").empty().html($(this).text());
    //             itemId = $(this).closest("li").attr("id");
    //             localStorage.setItem("megaMenuItem", itemId);
    //             window.megaModuleIds.push({
    //                 megaModuleId: currentmegaModuleId.megaModuleId,
    //                 megaMenuItem: itemId
    //             });
    //         });

    //         //$("#" + menuItemId + ">a").click();
    //     }

    // });
    $(window).scroll(function () {
        var top = $(window).scrollTop();
        if ($('.profile-img').length > 0) {
            $('.profile-img').css('top', top);
        }
        if ($('.nav-tabs').length > 0) {
        	if ($('.nav-tabs').parent().offset().top < top+30) {
	        	$('.nav-tabs,.btns-form').css('top', top-35);
        	}
        	else{
	        	$('.nav-tabs,.btns-form').css('top', '0');
        	}
        }
        else if($('.btns-form').length > 0){
        	if ($('.btns-form').parent().offset().top < top+30) {
	        	$('.btns-form').css('top', top-30);
        	}
        	else{
	        	$('.btns-form').css('top', '0');
        	}
        }
       if ($('.fileupload-buttonbar').length>0) {
           if ($('.fileupload-buttonbar').parent().parent().parent().offset.top < top + 60) {
                $('.fileupload-buttonbar').css('top', top - 60);
            }
            else {
                $('.fileupload-buttonbar').css('top','0')
            }
        }
    });

});