// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//************************************** */

//function AfisareDropdown(event) {
//    event.preventDefault();
//    document.getElementById("myDropdown").classList.toggle("show");
//}
//************************************** */



//window.onclick = function (event) {
//    if (!event.target.matches('.dropbtn')) {
//        var dropdowns = document.getElementsByClassName("dropdown-content");
//        var i;
//        for (i = 0; i < dropdowns.length; i++) {
//            var openDropdown = dropdowns[i];
//            if (openDropdown.classList.contains('show')) {
//                openDropdown.classList.remove('show');
//            }
//        }
//    }
//}



//************************************** */

//function setStatus(status) {
//    var statusButton = document.getElementById("statusButton");
//    statusButton.innerHTML = status; // Actualizare text buton "Status"

//    var statusField = document.getElementById("Status");
//    statusField.value = status; // Actualizare valoare câmp "Status"
//}

//************************************** */



//function checkInputLength() {
//    var input = document.getElementById("inputText");
//    var value = input.value.trim();

//    if (value.length === 3) {
//        console.log("Este linie");
//    } else {
//        console.log("Este cod");
//    }



//function updateFileName(input) {
//    var filename = input.files[0].name;
//    var fileinfo = document.getElementById("filename");
//    fileinfo.value = filename;

//    var checkImage = document.getElementById("checkImage");
//    checkImage.style.display = "block"; // Afișăm imaginea "check" când a fost selectat un fișier
//}






//function updateSelectedRow(input) {
//    var table = document.querySelector(".table table");
//    if (table) {
//        var filteredRows = table.querySelectorAll("tr");
//        var inputValue = input.value.trim().toLowerCase();
//        var selectedRow = null;

//        filteredRows.forEach(function (row) {
//            var firstCellText = row.cells[0].innerText.trim().toLowerCase();
//            if (firstCellText === inputValue) {
//                selectedRow = row;
//            }
//        });

//        if (selectedRow) {
//            var selectedValue = selectedRow.cells[0].innerText;
//            document.getElementById("selectedRowDiv").innerText = selectedValue;
//        } else {
//            document.getElementById("selectedRowDiv").innerText = "";
//        }
//    }
//}
//
//