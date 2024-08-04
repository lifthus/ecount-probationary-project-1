import { getItems } from "./api/items.js";

async function renderItemTable() {
    let itemTableBody = document.getElementById("item-table-body");
    const page = Number(new URLSearchParams(window.location.search).get("page"));
    const queryItemResp = await getItems(page || 1);
    const items = queryItemResp.items;

    items.forEach((item) => {
        itemTableBody.innerHTML += `
    <tr>
        <td>
            <input type="checkbox" />
        </td>
        <td>
            <a href="https://google.com">
                ${item.code}
            </a>
        </td>
        <td>
            ${item.name}
        </td>
        <td>
        <a href="https://google.com">
            수정
        </a>
        </td>
    </tr>
        `;
    });
}

renderItemTable();
