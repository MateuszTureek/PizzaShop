/* READY */
$(document).ready(function () {
    $('#Container').toggle('fade', '500');
    carouselHandling.init();
    //naviagtion.js
    //=============
    var navHandlingMainNav = { navHandling: navigationHandling };
    var navHandlingMenuNav = { navHandling: navigationHandling };
    navHandlingMainNav.navHandling.init({
        $navItem: $('#MainNav').children('li')
    });
    navHandlingMenuNav.navHandling.init({
        $navItem: $('#MenuItems').find('li')
    });
    //============
    var announcementBox = {
        showBox: showBox
    };
    announcementBox.showBox.init({
        box: $('#Announcement'),
        animType: 'drop',
        duration: '500',
        scrollTop: '230'
    });
});

//obsługa karuzeli
var carouselHandling = {
    init: function (settings) {
        carouselHandling.config = {
            $carousel: $('#Carousel'),
            $items: $('#Carousel').find('div').filter('.item'),
            activeClass: 'active'
        };
        $.extend(carouselHandling.config, settings);
        carouselHandling.activeFirstItem();
    },
    //ustawienie pierwszego elementu jako aktywny
    activeFirstItem: function () {
        carouselHandling.config.$items.first().addClass(carouselHandling.config.activeClass);
    }
};

//wymagana biblioteka jquery ui
//pokazywanie box o określonym id, wzgledem scroll top
var showBox = {
    init: function (settings) {
        showBox.config = {
            box: $('#Box'),
            animType: 'fade',
            duration: '500',
            scrollTop: '0'
        };
        $.extend(showBox.config, settings);
        //register events
        $(window).on('scroll.myScrollEvent', showBox.onScroll);
    },
    show: function () {
        showBox.config.box.toggle(showBox.config.animType, showBox.config.duration);
    },
    onScroll: function (e) {
        if ($(document).scrollTop() > showBox.config.scrollTop) {
            showBox.show();
            $(window).off('scroll.myScrollEvent');
        }
    }
};