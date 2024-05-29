document.addEventListener('DOMContentLoaded', function () {
    var teamNameInput = document.getElementById('TeamName');
    var teamsSelect = document.getElementById('teams');
    var addNewTeamBtn = document.getElementById('addNewTeam');

    if (teamNameInput.value.trim() != "") {
        addNewTeamBtn.style.display = 'block';
        addNewTeamBtn.disabled = false;
    }

    teamNameInput.addEventListener('input', function () {
        var selectedTeam = teamNameInput.value;
        if (selectedTeam.trim() != "") {
            if (!Array.from(teamsSelect.options).some(option => option.value === selectedTeam)) {
                teamNameInput.style.borderColor = 'red';
                addNewTeamBtn.style.display = 'block';
                addNewTeamBtn.disabled = false;
            } else {
                teamNameInput.style.borderColor = 'green';
                addNewTeamBtn.style.display = 'none';
                addNewTeamBtn.disabled = true;
            }
        } else {
            addNewTeamBtn.style.display = 'none';
            addNewTeamBtn.disabled = true;
        }

    });

    addNewTeamBtn.addEventListener('click', function () {
        event.preventDefault();

        if (teamsSelect) {
            fetch('/Home/AddTeam', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ id: teamsSelect.value, name: teamNameInput.value })
            })
                .then(function (response) {
                    if (!response.ok) {
                        throw new Error('Ошибка HTTP ' + response.status);
                    }
                    return response.json();
                })
                .then(function (data) {
                    var option = document.createElement('option');
                    option.text = data.id;
                    option.value = data.name;
                    teamsSelect.appendChild(option);
                    teamNameInput.value = data.name;
                    teamNameInput.style.borderColor = 'green';
                    addNewTeamBtn.style.display = 'none';

                })
                .catch(function (error) {
                    console.error('Ошибка при добавлении новой команды:', error);
                });
        }
    });
});