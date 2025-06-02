document.addEventListener('DOMContentLoaded', () => {

    var checkAll = document.getElementById("CheckAll");
    if (checkAll) {
        checkAll.addEventListener('change', (e) => {
            document
                .querySelectorAll('input[name="AreChecked"]')
                .forEach(cb => cb.checked = e.target.checked);
        });
    }

});