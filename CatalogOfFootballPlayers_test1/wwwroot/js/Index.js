const connection = new signalR.HubConnectionBuilder()
    .withUrl("/footballersHub")
    .build();

connection.on("ReceivefootballersUpdate", function (footballer) {
    console.log(footballer);

    var table = document.querySelector('table');
    var rows = table.querySelectorAll('tbody tr');

    let existingFootballerRow = null;

    rows.forEach(function (row) {
        var cells = row.querySelectorAll('td');

        if (cells[0].textContent === footballer.id.toString()) {
            existingFootballerRow = row;
        }
    });

    var formattedBirthDate = new Date(footballer.birthDate);

    function formatDate(date) {
        const d = new Date(date);
        const year = d.getFullYear();
        const month = String(d.getMonth() + 1).padStart(2, '0');
        const day = String(d.getDate()).padStart(2, '0');

        return `${year}-${month}-${day}`;
    }

    if (existingFootballerRow) {
        existingFootballerRow.innerHTML = `
                                    <td>${footballer.id}</td>
                                    <td>${footballer.firstName}</td>
                                    <td>${footballer.lastName}</td>
                                    <td>${footballer.gender}</td>
                                            <td>${formatDate(footballer.birthDate)}</td>
                                    <td>${footballer.team.name}</td>
                                    <td>${footballer.country}</td>
            <td><a class="btn btn-primary" href="/Home/EditFootballer/${footballer.id}">Редактировать</a></td>
                                             `;
    } else {
        const tbody = document.querySelector("tbody");
        const newRow = document.createElement("tr");
        newRow.innerHTML = `
                                    <td>${footballer.id}</td>
                                    <td>${footballer.firstName}</td>
                                    <td>${footballer.lastName}</td>
                                    <td>${footballer.gender}</td>
                                            <td>${formatDate(footballer.birthDate)}</td>
                                    <td>${footballer.team.name}</td>
                                    <td>${footballer.country}</td>
            <td><a class="btn btn-primary" href="/Home/EditFootballer/${footballer.id}">Редактировать</a></td>
                                             `;
        tbody.appendChild(newRow);
    }
});

connection.start()
    .then(function () {
        console.log("SignalR connected");
    }).catch(function (err) { console.error(err.toString()) });