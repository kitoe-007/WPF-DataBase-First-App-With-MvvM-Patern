CREATE TABLE suppliers (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    address TEXT,
    phone TEXT,
    contract_date DATETIME
);

CREATE TABLE products (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    type TEXT,
    manufacturer TEXT,
    storage_conditions TEXT,
    packaging TEXT,
    shelf_life INTEGER CHECK (shelf_life > 0)
);

CREATE TABLE warehouse (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    receipt_date DATETIME NOT NULL,
    product_id INTEGER NOT NULL,
    supplier_id INTEGER NOT NULL,
    delivery_method TEXT,
    volume INTEGER NOT NULL CHECK (volume > 0),
    price REAL CHECK (price > 0),
    FOREIGN KEY (product_id) REFERENCES products(id),
    FOREIGN KEY (supplier_id) REFERENCES suppliers(id)
);

INSERT INTO suppliers (name, address, phone, contract_date) VALUES
('ООО Альфа', 'г. Москва, ул. Ленина, 1', '+7 900 111-11-11', '2025-03-10 00:00:00'),
('ООО Бета', 'г. Тверь, ул. Мира, 5', '+7 900 222-22-22', '2025-11-01 00:00:00'),
('ИП Сидоров', 'г. Клин, ул. Садовая, 10', '+7 900 333-33-33', '2026-06-15 00:00:00');

INSERT INTO products (name, type, manufacturer, storage_conditions, packaging, shelf_life) VALUES
('Молоко', 'Молочные', 'Простоквашино', '+2..+6', 'Тетрапак', 10),
('Сахар', 'Бакалея', 'Русагро', 'Сухое место', 'Мешок', 365),
('Гречка', 'Бакалея', 'Мистраль', 'Сухое место', 'Пакет', 540),
('Сок яблочный', 'Напитки', 'Добрый', 'до +25', 'Тетрапак', 180);

INSERT INTO warehouse (receipt_date, product_id, supplier_id, delivery_method, volume, price) VALUES
('2026-09-01 00:00:00', 1, 1, 'Автотранспорт', 100, 80),
('2026-09-15 00:00:00', 2, 2, 'Ж/д', 500, 60),
('2026-10-01 00:00:00', 3, 2, 'Автотранспорт', 200, 90),
('2026-10-10 00:00:00', 4, 3, 'Самовывоз', 150, 120),
('2026-10-20 00:00:00', 1, 3, 'Самовывоз', 50, 85);
