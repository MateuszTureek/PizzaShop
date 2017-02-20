
$(document).ready(function () {
    siteModule.init();
});

var siteModule = (function () {
    //var
    var $CarouselSection = $('#Carousel').find('div').filter('.item');
    var $MainNavLi = $("#MainNav").children('li');
    var $MenuLi = $('#MenuItems').find('li');
    var $LiList = $($MainNavLi).add($MenuLi);
    //func
    var init = function ()   {
        //efekt pojawienia się strony
        $('#Container').toggle('fade', 500);
        //ustawienie pierwszej pozycji karuzeli na aktuktywną (class='active')
        $($CarouselSection).first().addClass('active');
        //obsługa oznaczania linków jako aktywnych (class='active')
        setActiveLink($LiList);
        $(window).on('scroll.my', function (e) {
            if ($(document).scrollTop() > 250) { showNews(); $(window).off('scroll.my'); }
        });
    };
    //dodanie klasy active do podanego elementu z listy
    var setActiveLink = function ($liList) {
        var currentHref = window.location.href;
        $($liList).each(function (index, $li) {
            var linkHref = $($li).children('a').attr('href');
            if (currentHref.indexOf(linkHref) !== -1 && linkHref !== '/') {
                $($li).siblings().removeClass('active');
                $($li).addClass('active');
                return;
            }
            if (currentHref.lastIndexOf(linkHref) === currentHref.length - 1) {
                $($LiList).first().addClass('active');
            }
        });
    };
    var showNews = function () {
        var $Announcement = $("#Announcement");
        $($Announcement).toggle('drop', 1000);
    };
    //public
    return {
        init: init
    };
})();