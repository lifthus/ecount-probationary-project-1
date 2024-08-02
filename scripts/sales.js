const sales = [
    ["2023-24", "P12345", "이카운트", 10, 40000, "이알피"],
    ["2023-24", "P12345", "이카운트", 10, 40000, "이알피"],
    ["2023-24", "P12345", "이카운트", 10, 40000, "이알피"],
];

const salesTableBody = document.getElementById("sales-table-body");

sales.forEach((sale) => {
    salesTableBody.innerHTML += `
<tr>
    <td>
        <input type="checkbox" />
    </td>
    <td>
    <a href="https://google.com">
        ${sale[0]}
    </a>
    </td>
    <td>${sale[1]}</td>
    <td>${sale[2]}</td>
    <td>${sale[3]}</td>
    <td>${sale[4]}</td>
    <td>${sale[5]}</td>
</tr>
    `;
});
