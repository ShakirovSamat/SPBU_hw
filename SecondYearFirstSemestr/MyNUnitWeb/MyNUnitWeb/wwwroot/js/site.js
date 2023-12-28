window.onload = init;

function init() {
    let btn = document.getElementById("btn");
    btn.onclick = async function () {
        let response = await fetch("http://localhost:5299/", {
            method: "POST",
            headers: {
                'Content-Type': 'text/html; charset=utf-8'
            },
            body: "Start",
        });
        alert(response.status);
    }    
}
