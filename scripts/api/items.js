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
    DB_ITEM.push(item);
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

export async function getItems(page = 1, pageSize = 10) {
    return new QueryItemsResponseDTO({
        page: page,
        totalPages: Math.ceil(DB_ITEM.length / pageSize),
        items: DB_ITEM.slice((page - 1) * pageSize, page * pageSize),
    });
}

/**
 *
 * @param {ItemDTO} item
 */
export async function updateItem(item) {
    const index = DB_ITEM.findIndex((it) => it.code === item.code);
    if (index < 0) return;
    DB_ITEM[index] = item;
}

/**
 *
 * @param {string[]} codes
 */
export async function deleteItems(codes) {
    DB_ITEM = DB_ITEM.filter((it) => !codes.includes(it.code));
}

// ========== fake server ==========

/**
 * @type {ItemDTO[]}
 */
const DB_ITEM = [
    new ItemDTO("P12345", "상품1"),
    new ItemDTO("P12346", "상품2"),
    new ItemDTO("P12347", "상품3"),
    new ItemDTO("P12348", "상품4"),
    new ItemDTO("P12349", "상품5"),
    new ItemDTO("P12350", "상품6"),
    // new ItemDTO("P12351", "상품7"),
    // new ItemDTO("P12352", "상품8"),
    // new ItemDTO("P12353", "상품9"),
    // new ItemDTO("P12354", "상품10"),
    // new ItemDTO("P12355", "상품11"),
    // new ItemDTO("P12356", "상품12"),
];
