/* READY */
$(document).ready(function () {
    // All items height setting on max height.
    var news = {
        findMaxHeight: findMaxHeight
    };
    news.findMaxHeight.init({
        elementsWithHeight: $('#News').find('.col-md-4')
    });
    var events = {
        findMaxHeight: findMaxHeight
    };
    events.findMaxHeight.init({
        elementsWithHeight: $('#Events').find('.col-md-4')
    });
    // ============
    // modalForm.js
    modalFormFromTable.init({
        $modal: $('#ModalBox')
    });
    // navigation.js
    // ============
    navigationHandling.init({
        $navItem: $('#AdminMenuCategories').find('li')
    });
    // ============
    var $menuList = $('#AdminMenuCategories');
    checkSublistItem.init({
        $menuList: $menuList,
        $sublistActivator: $($menuList).children('li').filter('.dropdown')
    });
});

var findMaxHeight = {
    heightTable: [],
    init: function (settings) {
        findMaxHeight.config = {
            elementsWithHeight: $('.elementWithHeight')
        };
        $.extend(findMaxHeight.config, settings);
        findMaxHeight.config.elementsWithHeight.height(findMaxHeight.getMaxHeight());
    },
    getMaxHeight: function () {
        for (var i = 0; i < findMaxHeight.config.elementsWithHeight.length; ++i) {
            findMaxHeight.heightTable[i] = findMaxHeight.config.elementsWithHeight.height();
        }
        var maxHeight = Math.max.apply(maxHeight, findMaxHeight.heightTable);
        return maxHeight;
    }
};

var checkSublistItem = {
    init: function (settings) {
        checkSublistItem.config = {
            has: 'active',
            $menuList: $('#Menu'), //menu główne z podmenu w sobie
            $sublistActivator: $('#Activator')
        };
        $.extend(checkSublistItem.config, settings);
        checkSublistItem.checkSublist();
        $(checkSublistItem.config.$sublistActivator).one('click', function (e) {
            $(this).find('ul').removeClass('hide');
        });
        checkSublistItem.config.$sublistActivator.on('click', checkSublistItem.sublistActivatorOnClick);
    },
    checkSublist: function () {
        if (checkSublistItem.config.$sublistActivator.find('ul').children('li').hasClass('active')) {
            checkSublistItem.config.$sublistActivator.find('ul').removeClass('hide');
        }
    },
    sublistActivatorOnClick: function () {
        e.preventDefault();
        checkSublistItem.config.$sublistActivator.find('ul').removeClass('hide');
    }
};