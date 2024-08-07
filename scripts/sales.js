import { SalesQueryRequestDTO } from "./api/sales.js";
import { querySales } from "/scripts/api/sales.js";

/**
 *
 * @param {import("./api/sales.js").SaleDTO[]} sales
 */
function _renderSalesTable(sales) {
    const salesTableBody = document.getElementById("sales-table-body");
    salesTableBody.innerHTML = "";

    sales.forEach((s) => {
        salesTableBody.innerHTML += `
    <tr>
        <td>
            <input type="checkbox" data-sale-number="${s.saleNumber}"/>
        </td>
        <td>
        <a class="text-blue" onclick="openPopup('/popups/sale-update-input.html?saleNumber=${s.saleNumber}')">
            ${s.saleDate.toISOString().slice(0, 10).replaceAll("-", "/")}-${s.saleNumber}
        </a>
        </td>
        <td>${s.itemCode}</td>
        <td>${s.itemName}</td>
        <td>${s.amount}</td>
        <td>${s.unitPrice}</td>
        <td>${s.briefs}</td>
    </tr>
        `;
    });
}

let currentPage = 1;
let totalPages = 1;
let currentStartDate = undefined;
let currentEndDate = undefined;
let currentSearchItemCodes = "";
let currentSearchBriefs = "";

const salesResp = await querySales({ page: 1, pageSize: 10 });
totalPages = salesResp.totalPages;
currentStartDate = salesResp.startDate;
currentEndDate = salesResp.endDate;
currentSearchItemCodes = salesResp.itemCodes;
currentSearchBriefs = salesResp.briefs;
_renderSalesTable(salesResp.sales);

export async function onSearchSales() {
    const salesFilterForm = document.getElementById("sales-filter-form");
    const formData = new FormData(salesFilterForm);
    const startSalesYear = formData.get("start-sales-year");
    const startSalesMonth = formData.get("start-sales-month");
    const startSalesDay = formData.get("start-sales-day");
    const endSalesYear = formData.get("end-sales-year");
    const endSalesMonth = formData.get("end-sales-month");
    const endSalesDay = formData.get("end-sales-day");
    const itemCodes = formData.get("sales-item-codes");
    const briefs = formData.get("sales-briefs");

    const startDate = new Date(startSalesYear, startSalesMonth - 1, startSalesDay);
    const endDate = new Date(endSalesYear, endSalesMonth - 1, endSalesDay);

    const dto = new SalesQueryRequestDTO(startDate, endDate, itemCodes, briefs, 1, 10);

    const resp = await querySales(dto);
    totalPages = resp.totalPages;
    currentStartDate = resp.startDate;
    currentEndDate = resp.endDate;
    currentSearchItemCodes = resp.itemCodes;
    currentSearchBriefs = resp.briefs;
    currentPage = 1;

    _renderSalesTable(resp.sales);
}

export async function onPrevSalesPage() {
    if (currentPage === 1) return;
    currentPage -= 1;
    const searchSalesResp = await querySales({
        startDate: currentStartDate,
        endDate: currentEndDate,
        itemCodes: currentSearchItemCodes,
        briefs: currentSearchBriefs,
        page: currentPage,
        pageSize: 10,
    });
    totalPages = searchSalesResp.totalPages;
    currentStartDate = searchSalesResp.startDate;
    currentEndDate = searchSalesResp.endDate;
    currentSearchItemCodes = searchSalesResp.itemCodes;
    currentSearchBriefs = searchSalesResp.briefs;
    const sales = searchSalesResp.sales;
    _renderSalesTable(sales);
}

export async function onNextSalesPage() {
    if (currentPage === totalPages) return;
    currentPage += 1;
    const searchSalesResp = await querySales({
        startDate: currentStartDate,
        endDate: currentEndDate,
        itemCodes: currentSearchItemCodes,
        briefs: currentSearchBriefs,
        page: currentPage,
        pageSize: 10,
    });
    totalPages = searchSalesResp.totalPages;
    currentStartDate = searchSalesResp.startDate;
    currentEndDate = searchSalesResp.endDate;
    currentSearchItemCodes = searchSalesResp.itemCodes;
    currentSearchBriefs = searchSalesResp.briefs;
    const sales = searchSalesResp.sales;
    _renderSalesTable(sales);
}
