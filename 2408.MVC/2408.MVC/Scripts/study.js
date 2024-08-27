document.addEventListener('DOMContentLoaded', function () {
    const selectButton = document.getElementById('study1');
    let data = { Name: '정훈', Age: 26 };

    selectButton.addEventListener('click', function () {
        fetch('/Study/Select', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        })
            .then(resp => resp.json())
            .then(data => {
                if (data.isSuccess) {
                    alert('SUCCESS');
                } else {
                    alert("FAIL");
                }
            })
            .catch(err => {
                console.error('error', err);
                alert('An error occured');
            });
    });
});