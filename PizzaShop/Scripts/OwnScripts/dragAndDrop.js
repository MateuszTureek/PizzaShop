
$(document).ready(function () {
    dragAndDrop.init({
        draggable: $(".draggable"),
        droppable: $("#Droppable"),
        accept: '.draggable',
        form: $('#PizzaForm')
    });
});

var dragAndDrop = {
    init: function (settings) {
        dragAndDrop.config = {
            draggable: $("#Draggable"), //draggable (Ul)
            droppable: $("#Droppable"), //droppable (Ul)
            accept: '.toAccept', //accept element for droppable
            form: $('#Form') //pizza form for create and edit
        };
        $.extend(dragAndDrop.config, settings);
        dragAndDrop.setup();
    },
    setup: function () {
        dragAndDrop.config.form.on('submit', dragAndDrop.onSubmitForm);
        dragAndDrop.config.droppable.children('li').find('div').on('click', dragAndDrop.closeOnClick);
        dragAndDrop.config.draggable.draggable({
            revert: 'invalid',
            drag: dragAndDrop.onDrag
        });
        dragAndDrop.config.droppable.droppable({
            accept: dragAndDrop.config.accept,
            drop: dragAndDrop.onDrop
        });
    },
    onSubmitForm: function (e) {
        //e.preventDefault();
        //droppable items - here are added input hidden with component its
        var $li = dragAndDrop.config.droppable.children('li');
        var id = -1;
        for (var i = 0; i < $($li).length; ++i) {
            id = $($li[i]).data('id');
            $("<input type='hidden' name='Components[" + i + "]' value='" + id + "' />").appendTo($($li)[i]);
        }
    },
    onDrop: function (event, ui) {
        var $item = $(ui.draggable);
        var id = $($item).data('id');
        var index = $(dragAndDrop.config.droppable).children().length;
        dragAndDrop.config.droppable.
            append("<li data-id=" + id + " class='droped'>" +
                   $($item).text() + "<div class='close'><span>&times;</span></div>" + "</li>");
        $($item).remove();
        dragAndDrop.config.droppable.children('li').find('div').on('click', dragAndDrop.closeOnClick);
        dragAndDrop.config.draggable.find(ui.draggable).remove();
        //alert($('#Droppable').html());
    },
    closeOnClick: function () {
        var $li = $(this).closest('li');
        var id = $($li).data('id');
        dragAndDrop.config.draggable.parent().
            append('<li data-id=' + id + ' class="draggable label label-default">' +
                   $($li).clone().children().remove().end().text() + '</li>');
        dragAndDrop.config.droppable.find($li).remove();
        $(dragAndDrop.config.accept).draggable();
        //alert($('#Draggable').html());
    },
    onDrag: function (event, ui) {
        dragAndDrop.config.droppable.addClass('onDrag');
        dragAndDrop.config.droppable.removeClass('droppable');
        dragAndDrop.config.droppable.children('li').find('div').off('click');
    }
};
