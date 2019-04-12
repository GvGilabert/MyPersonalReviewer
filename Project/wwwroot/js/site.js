$(document).ready(function(){
    $("#inputAddressAddPlace").on("change",function()
    {
        enableBtn($("#openPopUp"));
    })
})

function enableBtn(btn)
{
    if($("#inputAddressAddPlace").val()!="" && $("#inputNameAddPlace").val()!="")
    {
        $(btn).removeAttr("disabled");
    }else
    {
        $(btn).prop("disabled",true);
    }
}

function AddressPartial() {
    let addressString = document.getElementById("inputAddressAddPlace").value;
    $.ajax(
        {
            type: "POST",
            url: "../MyPlaces/AddressPartial",
            data: { address: addressString },
            success: function (data) { $('.modal-body').html(data); },
            error: function (data) { $('.modal-body').html("Please complete the address field!"); }
        });
}

function MenuPartial(idG) {
    $.ajax(
        {
            type: "POST",
            url: "../MyPlaces/MenuItemsPartial",
            data: { placeId: idG },
            success: function (data) { $('.menuItemsPar').html(data); },
            error: function (data) { $('.menuItemsPar').html("Error!"); }
        });
}

$(document).on('change', ':radio[name="Adrs"]', function () {
    var arOfVals = $(this).parent().nextAll().map(function () {
        return $(this).text();
    }).get();
    document.getElementById("inputAddressAddPlace").value = arOfVals;
    document.getElementById("LatLongEntered").value =
    document.getElementById("latLng").value;
});

let addTodoBtn = document.getElementById("addTodoBtn");
if(addTodoBtn != null)
{
    addTodoBtn.addEventListener("click", function () {
        if(document.getElementById("inputAddressAddPlace").value != "")
        { 
            disableThis(addTodoBtn);
            document.getElementById("AddPlaceAction").submit();
        }else
        {
            this.innerHTML ="Seleccione una dirección!";
        }
    });
}
function disableThis(a)
{
    a.disabled = true;
}

let ElementToDlt;
$(document).on('click',':button[name="deleteBtn"]',function()
{ 
    ElementToDlt = $(this).parent().find('form[name="deleteBtnF"]');
});

function deletePlace(b)
{
    disableThis(b);
    $(ElementToDlt).submit();
}

function submitForm(s)
{
    $("#"+s).submit();
}

$(document).on('click', ':button[name="openMenuBtn"]', 
function () {
    let n = $(this).parent().find('input[name="nameOfPlace"]').val();
    let id = $(this).parent().find('input[name="idOfPlace"]').val();
    $("#menuItemPlace").text(n);
    $("#menuItemId").val(id);
    MenuPartial(id);
    
});