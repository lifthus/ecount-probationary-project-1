export class SaleDTO {
    /**
     * @type {Date}
     */
    saleDate;
    /**
     * @type {number}
     */
    saleNumber;
    /**
     * @type {string}
     */
    itemCode;
    /**
     * @type {string}
     */
    itemName;
    /**
     * @type {number}
     */
    amount;
    /**
     * @type {string}
     */
    unitPrice;
    /**
     * @type {string}
     */
    briefs;
    constructor(saleDate, saleNumber, itemCode, amount, unitPrice, briefs) {
        this.saleDate = new Date(saleDate);
        this.saleNumber = saleNumber;
        this.itemCode = itemCode;
        this.amount = amount;
        this.unitPrice = unitPrice;
        this.briefs = briefs;

        this.itemName = "JOIN";
    }
}
export class SalesQueryRequestDTO {
    /**
     *
     * @param {Date} startDate
     * @param {Date} endDate
     * @param {string} itemCodes
     * @param {string} briefs
     * @param {number} page
     * @param {number} pageSize
     */
    constructor(startDate, endDate, itemCodes, briefs, page, pageSize) {
        /**
         * @type {Date}
         */
        this.startDate = new Date(startDate);
        /**
         * @type {Date}
         */
        this.endDate = new Date(endDate);
        /**
         * @type {string}
         */
        this.itemCodes = itemCodes;
        /**
         * @type {string}
         * */
        this.briefs = briefs;
        /**
         * @type {number}
         */
        this.page = page || 1;
        /**
         * @type {number}
         */
        this.pageSize = pageSize || 10;
    }
}

export class QuerySalesResponseDTO {
    constructor(resp) {
        /**
         * @type {number}
         */
        this.page = resp.page;
        /**
         * @type {number}
         */
        this.totalPages = resp.totalPages;
        /**
         * @type {SaleDTO[]}
         */
        this.sales = resp.sales.map((s) => new SaleDTO(s.saleDate, s.saleNumber, s.itemCode, s.amount, s.unitPrice, s.briefs));
        /**
         * @type {Date}
         */
        this.startDate = resp.startDate;
        /**
         * @type {Date}
         */
        this.endDate = resp.endDate;
        /**
         * @type {string}
         */
        this.itemCodes = resp.itemCodes;
        /**
         * @type {string}
         */
        this.briefs = resp.briefs;
    }
}

/**
 *
 * @param {SalesQueryRequestDTO} dto
 * @return {SaleDTO[]}
 */
export function querySales(dto) {
    dto.page = dto.page || 1;
    dto.pageSize = dto.pageSize || 10;
    if (!dto.startDate || dto.startDate.toString() === "Invalid Date") dto.startDate = undefined;
    if (!dto.endDate || dto.endDate.toString() === "Invalid Date") dto.endDate = undefined;

    return getSalesLS(dto);
}
/**
 *
 * @param {string} saleDate
 * @param {number} saleNumber
 * @returns
 */
export async function querySale(saleNumber) {
    return querySaleLS(saleNumber);
}
/**
 *
 * @param {SaleDTO} sale
 * @returns
 */
export function createSale(sale) {
    if (!sale || sale.saleNumber !== undefined) return null;
    return createSaleLS(sale);
}

/**
 *
 * @param {SaleDTO} sale
 * @returns
 */
export function updateSale(sale) {
    sale.saleDate = undefined;
    return updateSaleLS(sale);
}

export function deleteSale(saleNumber) {
    deleteSaleLS(saleNumber);
}

export function deleteSales(saleNumbers) {
    deleteSalesLS(saleNumbers);
}

// ========== fake server ==========

/**
 *
 * @param {SalesQueryRequestDTO} dto
 */
function getSalesLS(dto) {
    /**
     * @type {SaleDTO[]}
     */
    const rawSales = JSON.parse(localStorage.getItem("DB_SALE")) || [];
    const sales = rawSales.map((rs) => new SaleDTO(rs.saleDate, rs.saleNumber, rs.itemCode, rs.amount, rs.unitPrice, rs.briefs));
    const filteredSales = sales.filter((s) => {
        if (dto.startDate && s.saleDate < dto.startDate) return false;
        if (dto.endDate && s.saleDate > dto.endDate) return false;
        const dtoItemCodeStr = dto.itemCodes || "";
        const itemCodes = dtoItemCodeStr.split(",");
        const filteredItemCodes = itemCodes.filter((ic) => ic.trim() !== "");
        if (filteredItemCodes.length > 0 && !filteredItemCodes.includes(s.itemCode)) return false;
        if (dto.briefs && !s.briefs.includes(dto.briefs)) return false;
        return true;
    });
    const page = dto.page;
    const pageSize = dto.pageSize;
    return new QuerySalesResponseDTO({
        page: page || 0,
        totalPages: Math.ceil(filteredSales.length / pageSize) || 0,
        sales: filteredSales.slice((page - 1) * pageSize, page * pageSize),
        startDate: dto.startDate,
        endDate: dto.endDate,
        itemCodes: dto.itemCodes,
        briefs: dto.briefs,
    });
}
function querySaleLS(saleNumber) {
    saleNumber = Number(saleNumber);
    /**
     * @type {SaleDTO[]}
     */
    const rawSales = JSON.parse(localStorage.getItem("DB_SALE")) || [];
    const sales = rawSales.map((rs) => new SaleDTO(rs.saleDate, rs.saleNumber, rs.itemCode, rs.amount, rs.unitPrice, rs.briefs));
    const targetSale = sales.find((s) => s.saleNumber === saleNumber);
    return targetSale;
}
/**
 *
 * @param {SaleDTO} sale
 */
function createSaleLS(sale) {
    const dbSales = JSON.parse(localStorage.getItem("DB_SALE")) || [];
    const newSaleNumber = dbSales.length + 1;
    sale.saleNumber = newSaleNumber;
    dbSales.push(sale);
    localStorage.setItem("DB_SALE", JSON.stringify(dbSales));
    return sale;
}
/**
 *
 * @param {SaleDTO} sale
 */
function updateSaleLS(sale) {
    const dbSales = JSON.parse(localStorage.getItem("DB_SALE")) || [];
    const targetSale = dbSales.find((s) => s.saleNumber === sale.saleNumber);
    if (!targetSale) return null;
    sale.saleDate = targetSale.saleDate;
    Object.assign(targetSale, sale);
    console.log("AAA", JSON.stringify(dbSales));
    localStorage.setItem("DB_SALE", JSON.stringify(dbSales));
    return sale;
}
/**
 *
 * @param {string} dateNumber
 */
function deleteSaleLS(saleNumber) {
    const dbSales = JSON.parse(localStorage.getItem("DB_SALE")) || [];
    const filteredDbSales = dbSales.filter((s) => s.saleNumber !== saleNumber);
    localStorage.setItem("DB_SALE", JSON.stringify(filteredDbSales));
}

function deleteSalesLS(saleNumbers) {
    const dbSales = JSON.parse(localStorage.getItem("DB_SALE")) || [];
    const filteredDbSales = dbSales.filter((s) => !saleNumbers.includes(s.saleNumber));
    localStorage.setItem("DB_SALE", JSON.stringify(filteredDbSales));
}
