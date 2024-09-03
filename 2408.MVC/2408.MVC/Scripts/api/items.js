import { API_PRODUCT_SELECT_PATH } from "../constant.js";

export class ItemDTO {
    /**
     * @param {string} com_code
     * @param {string} prod_cd
     * @param {string} prod_nm
     * @param {number} price
     * @param {string} write_dt
     */
    constructor(com_code, prod_cd, prod_nm, price, write_dt) {
        this.com_code = com_code;
        this.prod_cd = prod_cd;
        this.prod_nm = prod_nm;
        this.price = price;
        this.write_dt = write_dt;
    }
}

export class CreateProductRequestDTO {
    /**
     * @param {string} com_code
     * @param {string} prod_cd
     * @param {string} prod_nm
     * @param {number} price
     */
    constructor(com_code, prod_cd, prod_nm, price) {
        this.com_code = com_code;
        this.prod_cd = prod_cd;
        this.prod_nm = prod_nm;
        this.price = price;
    }
}


export class ItemQueryRequestDTO {
    /**
     * @param {string} itemCode
     * @param {string} itemName
     */
    constructor(itemCode, itemName) {
        /**
         * @type {string}
         */
        this.itemCode = itemCode;
        /**
         * @type {string}
         */
        this.itemName = itemName;
    }
}

export class QueryItemResponseDTO {
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
         * @type {ItemDTO[]}
         */
        this.items = resp.items.map((it) => new ItemDTO(it.code, it.name));
        /**
         * @type {string}
         */
        this.itemCode = resp.itemCode;
        /**
         * @type {string}
         */
        this.itemName = resp.itemName;
    }
}

export class ProductDTO {
    constructor(resp) {
        /**
         * @type {ProductKeyDTO}
         */
        this.Key = resp.Key;
        /**
         * @type {string}
         */
        this.PROD_NM = resp.PROD_NM;
        /**
         * @type {string}
         */
        this.PRICE = resp.PRICE;
        /**
         * @type {bool}
         */
        this.ACTIVE = resp.ACTIVE;
        /**
         * @type {string}
         */
        this.WRITE_DT = resp.WRITE_DT;
    }
}

export class ProductKeyDTO {
    constructor(key) {
        /**
         * @type {string}
         */
        this.COM_CODE = key.COM_CODE;
        /**
         * @type {string}
         */
        this.PROD_CD = key.PROD_CD;
    }
}

export class SelectProductRequestDTO {
    /**
     * @type {string}
     */
    COM_CODE = "";
    /**
     * @type {string}
     */
    PROD_CD = "";
    /**
     * @type {string}
     */
    PROD_NM = "";
    /**
     * @type {number}
     */
    ord_PROD_NM = 0;
    /**
     * @type {number}
     */
    ACTIVE = 1;
    /**
     * @type {number}
     */
    pageSize = 10;
    /**
     * @type {number}
     */
    pageNo = 1;
}

export class SelectProductResponseDTO {
    constructor(resp) {
        /**
         * @type {ProductDTO[]}
         */
        this.products = resp.products;
        /**
         * @type {number}
         */
        this.totalCount = resp.totalCount;
        /**
         * @type {number}
         */
        this.pageSize = resp.pageSize;
        /**
         * @type {number}
         */
        this.pageNo = resp.pageNo;
    }
}

/**
 * 
 * @param {SelectProductRequestDTO} dto
 * @return {SelectProductResponseDTO}
 */

export async function selectProducts(dto) {
    const queryString = new URLSearchParams(dto).toString();
    const resp = await fetch(API_PRODUCT_SELECT_PATH + '?' + queryString, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
        }
    });
    return resp.json();
}

/**
 *
 * @param {ItemQueryRequestDTO} dto
 * @return {QueryItemResponseDTO}
 */
export function queryItems(dto) {
    dto.page = dto.page || 1;
    dto.pageSize = dto.pageSize || 10;
    dto.itemCode = dto.itemCode || "";
    dto.itemName = dto.itemName || "";

    const resp = fetch
    return queryItemsLS(dto);
}

/**
 * @param {ItemDTO} item
 */
export async function createItem(item) {
    return createItemLS(item);
}

export class QueryItemsResponseDTO {
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
         * @type {ItemDTO[]}
         */
        this.items = resp.items.map((item) => new ItemDTO(item.code, item.name));
    }
}

export async function getItem(code) {
    const itemList = getItemsLS();
    const item = itemList.find((it) => it.code === code);
    if (!item) return null;
    return new ItemDTO(item.code, item.name);
}

export async function getItems(page = 1, pageSize = 10) {
    const itemList = getItemsLS();
    return new QueryItemsResponseDTO({
        page: page,
        totalPages: Math.ceil(itemList.length / pageSize),
        items: itemList.slice((page - 1) * pageSize, page * pageSize),
    });
}

/**
 *
 * @param {*} code
 * @param {*} name
 * @param {*} page
 * @param {*} pageSize
 * @returns {QueryItemsResponseDTO}
 */
export async function searchItems(code, name, page = 1, pageSize = 10) {
    if (!code) code = "";
    if (!name) name = "";
    const itemList = getItemsLS();
    const filteredItems = itemList.filter((item) => {
        return item.code.includes(code) && item.name.includes(name);
    });
    return new QueryItemsResponseDTO({
        page: page,
        totalPages: Math.ceil(filteredItems.length / pageSize),
        items: filteredItems.slice((page - 1) * pageSize, page * pageSize),
    });
}

/**
 *
 * @param {ItemDTO} item
 */
export async function updateItem(item) {
    return updateItemLS(item);
}

/**
 *
 * @param {string[]} codes
 */
export async function deleteItem(code) {
    return deleteItemLS(code);
}

export async function deleteItems(codes) {
    for (const code of codes) {
        deleteItemLS(code);
    }
}

// ========== fake server ==========

/**
 *
 * @param {ItemQueryRequestDTO} dto
 * @returns {QueryItemResponseDTO}
 */
function queryItemsLS(dto) {
    const items = getItemsLS();
    const filteredItems = items.filter((item) => {
        return item.code.includes(dto.itemCode) && item.name.includes(dto.itemName);
    });
    return new QueryItemResponseDTO({
        page: dto.page,
        totalPages: Math.ceil(filteredItems.length / dto.pageSize),
        items: filteredItems.slice((dto.page - 1) * dto.pageSize, dto.page * dto.pageSize).map((item) => new ItemDTO(item.code, item.name)),
        itemCode: dto.itemCode,
        itemName: dto.itemName,
    });
}

/**
 *
 * @returns {ItemDTO[]}
 */
function getItemsLS() {
    return JSON.parse(localStorage.getItem("DB_ITEM")) || [];
}

function createItemLS(item) {
    if (!item.code || !item.name) return;
    const itemList = getItemsLS();
    itemList.push(item);
    localStorage.setItem("DB_ITEM", JSON.stringify(itemList));
    return item;
}

function updateItemLS(itemDTO) {
    const itemList = getItemsLS();
    const item = itemList.find((it) => it.code === itemDTO.code);
    if (!item) return;
    item.name = itemDTO.name;
    localStorage.setItem("DB_ITEM", JSON.stringify(itemList));
    return item;
}

function deleteItemLS(code) {
    const itemList = getItemsLS();
    localStorage.setItem("DB_ITEM", JSON.stringify(itemList.filter((it) => code !== it.code)));
    return code;
}
