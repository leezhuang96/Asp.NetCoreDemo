let connection = null;

setupConnection = () => {
    //设置使用longPolling
    //connection = new signalR.HubConnectionBuilder()
    //    .withUrl("/counthub", signalR.HttpTransportType.LongPolling)
    //    .build();

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/counthub")
        .build();

    connection.on("ReciveUpdate", (update) => {
        const resultDiv = document.getElementById("result");
        resultDiv.innerHTML = update;
    });

    connection.on("someFunc", function (obj) {
        const resultDiv = document.getElementById("result");
        resultDiv.innerHTML = "Someone called, parametes: " + obj.random;
    });

    connection.on("Finsihed", function (obj) {
        const resultDiv = document.getElementById("result");
        resultDiv.innerHTML = "Finsihed";
    });

    connection.start()
        .catch(err => console.error(err.toString()));
}

setupConnection();

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();

    fetch("/api/count",
        {
            method: "POST",
            headers: {
                'content-type': 'application/json'
            }
        })
        .then(response => response.text())
        .then(maxValue => connection.invoke("GetLatestCount", maxValue));
})

