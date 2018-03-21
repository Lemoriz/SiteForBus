var idForUser, userAccess;

var host = "http://localhost:49529/api/";

levelAccess();

//Вывод найденных рейсов
function addRowFindTrip(date, freeSeats, modelBus, numBus, tripId) {
    // Находим нужную таблицу
    var tbody = document.getElementById('tab1').getElementsByTagName('TBODY')[0];

    // Создаем строку таблицы и добавляем ее
    var row = document.createElement("TR");
    row.addEventListener("click", o => {

        if(userAccess != "admin" && userAccess != "manager" && userAccess != "user")
    {
        alert("Зарегестрируйтесь для бронирования билетов");
        return ;
    }

        result = confirm("Забронировать рейс?");
        BookingTicket(result, tripId, idForUser);
    });
    tbody.appendChild(row);

    // Создаем ячейки в вышесозданной строке
    // и добавляем тх
    var td1 = document.createElement("TD");
    var td2 = document.createElement("TD");
    var td3 = document.createElement("TD");
    var td4 = document.createElement("TD");

    row.appendChild(td1);
    row.appendChild(td2);
    row.appendChild(td3);
    row.appendChild(td4);

    // Наполняем ячейки
    td1.innerHTML = date;
    td2.innerHTML = freeSeats;
    td3.innerHTML = modelBus;
    td4.innerHTML = numBus;
}

//Вывод забронированных пользователем маршрутов
function addRowBooking(date, begining, end, ticketId) {

    var tbody = document.getElementById('tab2').getElementsByTagName('TBODY')[0];

    var row = document.createElement("TR");
    tbody.appendChild(row);
    row.addEventListener("click", o => {

        if(userAccess != "admin" && userAccess != "manager" && userAccess != "user")
        {
            alert("Зарегестрируйтесь для отмены брони");
            return ;
        }

        result = confirm("Отменить рейс?");
        DeleteTicket(result, ticketId);
        console.log("sad");

    });

    var td1 = document.createElement("TD");
    var td2 = document.createElement("TD");
    var td3 = document.createElement("TD");

    row.appendChild(td1);
    row.appendChild(td2);
    row.appendChild(td3);

    td1.innerHTML = date;
    td2.innerHTML = begining;
    td3.innerHTML = end;
}

//Вывод графика работы определенного водителя
function addRowSchedule(date, licensePlate, begining, end) {

    var tbody = document.getElementById('tab3').getElementsByTagName('TBODY')[0];

    var row = document.createElement("TR");

    tbody.appendChild(row);

    var td1 = document.createElement("TD");
    var td2 = document.createElement("TD");
    var td3 = document.createElement("TD");
    var td4 = document.createElement("TD");

    row.appendChild(td1);
    row.appendChild(td2);
    row.appendChild(td3);
    row.appendChild(td4);

    td1.innerHTML = date;
    td2.innerHTML = licensePlate;
    td3.innerHTML = begining;
    td4.innerHTML = end;
}

var addRowItineraryId;
//Вывод всех маршрутов в базе
function addRowItinerary(begin, end, id) {

    var tbody = document.getElementById('tab4').getElementsByTagName('TBODY')[0];
    var table = document.getElementById("tab4");

    var row = document.createElement("TR");
    tbody.appendChild(row);

    row.addEventListener("click", o => {

        for (var i = 1; i < table.rows.length; i++) {
            if (table.rows[i].classList.contains("selected") == true) {
                table.rows[i].classList.toggle("selected");
            }
        }
        row.classList.toggle("selected");

        addRowItineraryId = id;
        document.getElementById("beginItinerary").value = begin;
        document.getElementById("endItinerary").value = end;
    });

    var td1 = document.createElement("TD");
    var td2 = document.createElement("TD");

    row.appendChild(td1);
    row.appendChild(td2);

    td1.innerHTML = begin;
    td2.innerHTML = end;
}

var IdSelectedDriver;
//Вывод всех водителей
function addRowAllBusDriver(typeBus, firstNameDriver, secondNameDriver, IdDriver) {

    var tbody = document.getElementById('tab5').getElementsByTagName('TBODY')[0];
    var table = document.getElementById("tab5");

    var row = document.createElement("TR");
    tbody.appendChild(row);

    row.addEventListener("click", o => {
        IdSelectedDriver = IdDriver;
        for (var i = 1; i < table.rows.length; i++) {
            if (table.rows[i].classList.contains("selected") == true) {
                table.rows[i].classList.toggle("selected");
            }
        }
        row.classList.toggle("selected");
    });

    var td1 = document.createElement("TD");
    var td2 = document.createElement("TD");
    var td3 = document.createElement("TD");

    row.appendChild(td1);
    row.appendChild(td2);
    row.appendChild(td3);

    td1.innerHTML = typeBus;
    td2.innerHTML = firstNameDriver;
    td3.innerHTML = secondNameDriver;
}

//Вывод всех типов автобуса
function addRowCopyOfBus(typeBus, licensePlate) {

    var tbody = document.getElementById('tab6').getElementsByTagName('TBODY')[0];

    var row = document.createElement("TR");
    tbody.appendChild(row);
    row.addEventListener("click", o => {

    });

    var td1 = document.createElement("TD");
    var td2 = document.createElement("TD");

    row.appendChild(td1);
    row.appendChild(td2);

    td1.innerHTML = typeBus;
    td2.innerHTML = licensePlate;
}

//Вставить значения в select
function InsertValueIntoSelect(input) {

    var select = document.getElementById("selectList"),
        txtVal = input,
        newOption = document.createElement("OPTION"),
        newOptionVal = document.createTextNode(txtVal);

    newOption.appendChild(newOptionVal);
    select.insertBefore(newOption, select.firstChild);
}

//Обнулить select
function DeleteSelect() {

    document.getElementById('selectList').innerHTML = null;
}

//Редактировать маршруты
function EditItinerary() {

    if(userAccess != "admin" && userAccess != "manager")
    {
        alert("Доступ к действию доступен только для администратора или менеджера");
        return ;
    }

    let obj = {
        beginningWay: document.getElementById("beginItinerary").value,
        endWay: document.getElementById("endItinerary").value
    }

    var str = host + "EditItinerary/" + addRowItineraryId;
    fetch(str, {
        method: 'put', headers: { "Content-type": "application/json; charset=UTF-8" },
        body: JSON.stringify(obj)
    }).then(q => { ViewItinerary() })
}

//Забронировать билет
function BookingTicket(result, idTrip, idUser) {

    if (result == false)
        return

    let obj = { tripId: idTrip, userId: idUser }

    var str = host + "Booking"

    fetch(str, {
        method: 'post', headers: { "Content-type": "application/json; charset=UTF-8" },
        body: JSON.stringify(obj)
    }).then(q => { FindTrip() })
}

//Отменить бронирование
function DeleteTicket(result, TicketId) {
    if (result == false)
        return

    var str = host + "Booking/" + TicketId;

    fetch(str, {
        method: 'delete', headers:
            { "Content-type": "application/json; charset=UTF-8" },
    }).then(o => BookingTrips());
}

//Найти рейс
function FindTrip() {

    clearTable('tab1');

    var pointA = document.getElementById("begin").value;
    var pointB = document.getElementById("end").value;

    var str = host + "FindAllInfTrip?Begining=" + pointA + "&End=" + pointB
    fetch(str).then(q => q.json()).then(q => q.forEach(element => {
        addRowFindTrip(element.date, element.viewBusSeats, element.busModel, element.licensePlate, element.id);
    }));
}

//Забронированные рейсы для пользователя
function BookingTrips() {
    clearTable('tab2');

    var str = host + "BookingTripForUser/" + idForUser;
    fetch(str).then(q => q.json()).then(q => q.forEach(element => {
        addRowBooking(element.date, element.begining, element.end, element.ticketId);
    }));
}

//График работы водителей
function ViewSchedule() {

    if(userAccess != "admin" && userAccess != "manager")
    {
        alert("Доступ к действию доступен только для администратора или менеджера");
        return ;
    }

    clearTable('tab3');

    var FirstName = document.getElementById("FirstNameDriver").value;
    var SecondName = document.getElementById("SecondNameDriver").value;

    var str = host + "FindSchedule?FirstName=" + FirstName + "&SecondName=" + SecondName;
    fetch(str).then(q => q.json()).then(q => q.forEach(element => {
        addRowSchedule(element.date, element.licensePlate, element.begining, element.end);
    }));
}

//Запрос на маршруты
function ViewItinerary() {
    clearTable('tab4');

    document.getElementById("beginItinerary").value = "";
    document.getElementById("endItinerary").value = "";

    var str = host + "EditItinerary";
    fetch(str).then(q => q.json()).then(q => q.forEach(element => {
        addRowItinerary(element.beginningWay, element.endWay, element.id);
    }));
}

//Вывод водителей и автобусов
function ViewTwoTable() {

    if(userAccess != "admin" && userAccess != "manager")
    {
        alert("Доступ к действию доступен только для администратора или менеджера");
        return ;
    }

    clearTable('tab5');

    clearTable('tab6');

    var str = host + "AllBUsDriver";
    fetch(str).then(q => q.json()).then(q => q.forEach(element => {
        addRowAllBusDriver(element.copyOfBusId, element.firstName, element.secondName, element.id);
    }));

    var str = host + "AllCopyOfBus";
    fetch(str).then(q => q.json()).then(q => q.forEach(element => {
        addRowCopyOfBus(element.id, element.licensePlate);
        InsertValueIntoSelect(element.id);
    }));

    DeleteSelect();
}

//Изменить привязку автобусов
function ChangeBusForDriver() {

    if(userAccess != "admin" && userAccess != "manager")
    {
        alert("Доступ к действию доступен только для администратора или менеджера");
        return ;
    }

    let obj = {
        copyOfBusId: document.getElementById("selectList").value,
    }

    var str = host + "ChangeDriverBus/" + IdSelectedDriver;
    fetch(str, {
        method: 'put', headers: { "Content-type": "application/json; charset=UTF-8" },
        body: JSON.stringify(obj)
    }).then(q => { ViewTwoTable() })
}

//Очистить таблицу
function clearTable(tabId) {
    for (; document.getElementById(tabId).getElementsByTagName('TR').length > 1;) {
        document.getElementById(tabId).deleteRow(1);
    }
}

//Релизация авторизации
function Authorization() {

    if ((document.getElementById("MailAuthoriz").value && document.getElementById("PasswordAuthoriz").value) == "") {
        alert("Заполните все поля помеченные *");
        return;
    }

    str1 = host + "DeleteCookie";

    fetch(str1).then(q=>{ SetNewCookie(
        document.getElementById("MailAuthoriz").value,
        document.getElementById("PasswordAuthoriz").value
    );});
}

//Разлогиниться
function DeleteCookie() {
    str1 = host + "DeleteCookie";

    fetch(str1, { credentials: "include" }).then(q=>{location.reload(true);});

}

//Проверка на вход
function CheckCookie() {
    var str3 = host + "GetSession";
    fetch(str3, { credentials: "include" }).
    then(q => q.json()).then(q => {

    
        if(document.getElementById('UserName') != null)
        {
            document.getElementById('UserName').innerText += q.firstName;
        }
        idForUser = q.id;
         userAccess = q.access;
        });
}

//Запрос на установку куки
function SetNewCookie(mail, password) {
    var str = host + "SetCookie?Mail=" + mail + "&Password=" + password;

    fetch(str, { credentials: "include" }
    ).then(o => {
        if (o.status == 400) {
            alert("Неверный логин или пароль")
            return;
        }
        o.json().then(q => { document.location.href = "index.html"; });
    }
        )
}

//Проверка доступа
function levelAccess() {

    var str = host + "GetSession";

    fetch(str, { credentials: "include" }).then(q => this.check(q));

    check = (q) => {
        if (q.status == 400) {
            console.log("bad request")
            document.getElementById('enter').style.display = 'block';
            document.getElementById('Exit').style.display = 'none';
        }

        else  {
            console.log("asdsda");
            q.json().then(  w => {CheckCookie()});
            document.getElementById('Exit').style.display = 'block';
            document.getElementById('enter').style.display = 'none';

        }
    }
}

//Регистрация нового пользователя
function UserReg() {
    if ((document.getElementById("FirstNameReg").value && document.getElementById("EmailReg").value &&
        document.getElementById("PasswordReg").value) == "") {
        alert("Заполните все поля помеченные *");
        return;
    }

    let obj = {
        FirstName: document.getElementById("FirstNameReg").value,
        Mail: document.getElementById("EmailReg").value,
        Password: document.getElementById("PasswordReg").value,
        SecondName: document.getElementById("SecondNameReg").value,
        TelephoneNumber: document.getElementById("TelephoneReg").value,
    }

    var str = host + "AddNewUser";

    var str1 = host + "DeleteCookie";

    var str3 = host + "SetCookie?Mail=" +  document.getElementById("EmailReg").value + 
    "&Password=" +  document.getElementById("PasswordReg").value;

   
    fetch(str, {method: 'post', headers: { "Content-type": "application/json; charset=UTF-8" },body: JSON.stringify(obj)}).
    then( q=>{ fetch(str1).
    then( q=> {fetch(str3, { credentials: "include" }).
    then(o => {
        if (o.status == 400) {
            alert("Неверный логин или пароль"); 
            return
        }o.json().then(q => 
            { 
                alert("Регистрация прошла успешно!")
                document.location.href = "index.html"; 
            });
    })}) })
}