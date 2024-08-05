export class ItemDTO {
    /**
     * @param {string} code
     * @param {string} name
     */
    constructor(code, name) {
        this.code = code;
        this.name = name;
    }
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

// ========== fake server ==========

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
    console.log("deleteItemLS", code);
    return code;
}
