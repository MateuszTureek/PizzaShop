
//obsłga nawigacji, zaznaczenie aktynego elementu
var navigationHandling = {
    init: function (settings) {
        navigationHandling.config = {
            $navItem: $('#Navigation').children('li')
        };
        $.extend(navigationHandling.config, settings);
        navigationHandling.setActiveLink();
    },
    setActiveLink: function () {
        var currentHref = window.location.href;
        navigationHandling.config.$navItem.each(function (index, $li) {
            var linkHref = $($li).children('a').attr('href');
            if (currentHref.indexOf(linkHref) !== -1 && linkHref !== '/') {
                $($li).siblings().removeClass('active');
                $($li).addClass('active');
                return;
            }
            if (currentHref.lastIndexOf(linkHref) === currentHref.length - 1) {
                $(navigationHandling.config.$navItem).first().addClass('active');
            }
        });
    }
};