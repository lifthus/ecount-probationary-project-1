import { getItems, searchItems } from "./api/items.js";

let currentPage = 1;
let totalPages = 1;
let currentSearchCode = "";
let currentSearchName = "";

export async function renderItemTable() {
    const page = Number(new URLSearchParams(window.location.search).get("page"));
    const queryItemResp = await getItems(page || 1);
    const items = queryItemResp.items;

    currentPage = queryItemResp.page;
    totalPages = queryItemResp.totalPages;
    currentSearchCode = "";
    currentSearchName = "";

    _renderItemTable(items);
}

export async function onSearch() {
    const searchForm = document.getElementById("item-search-form");
    const itemCode = searchForm.querySelector("input[name='item-code']").value;
    const itemName = searchForm.querySelector("input[name='item-name']").value;
    const searchItemsResp = await searchItems(itemCode, itemName);

    currentPage = searchItemsResp.page;
    totalPages = searchItemsResp.total;
    currentSearchCode = itemCode;
    currentSearchName = itemName;

    const items = searchItemsResp.items;

    _renderItemTable(items);
}

export async function onPrevPage() {
    if (currentPage === 1) return;
    currentPage -= 1;
    const searchItemsResp = await searchItems(currentSearchCode, currentSearchName, currentPage);
    totalPages = searchItemsResp.totalPages;
    const items = searchItemsResp.items;
    _renderItemTable(items);
}

export async function onNextPage() {
    if (currentPage === totalPages) return;
    currentPage += 1;
    const searchItemsResp = await searchItems(currentSearchCode, currentSearchName, currentPage);
    totalPages = searchItemsResp.totalPages;
    const items = searchItemsResp.items;
    _renderItemTable(items);
}

function _renderItemTable(items) {
    const itemTableBody = document.getElementById("item-table-body");
    itemTableBody.innerHTML = "";

    items.forEach((item) => {
        itemTableBody.innerHTML += `
    <tr>
        <td>
            <input type="checkbox" data-item-code="${item.code}" onclick="onCheckItem(event)" />
        </td>
        <td>
            <a class="text-blue">
                ${item.code}
            </a>
        </td>
        <td>
            ${item.name}
        </td>
        <td>
        <a class="text-blue" onclick="openPopup('/popups/item-update-input.html?code=${item.code}')">
            수정
        </a>
        </td>
    </tr>
        `;
    });
}
