--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.1
-- Dumped by pg_dump version 9.4.1
-- Started on 2015-04-27 18:33:06

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 180 (class 3079 OID 11855)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2036 (class 0 OID 0)
-- Dependencies: 180
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_with_oids = false;

--
-- TOC entry 175 (class 1259 OID 16555)
-- Name: productrecord; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE productrecord (
    productrecordid integer NOT NULL,
    productid integer NOT NULL,
    price integer NOT NULL,
    rating real,
    "timestamp" timestamp without time zone NOT NULL,
    amountavailable integer,
    description text,
    name text
);


--
-- TOC entry 176 (class 1259 OID 16561)
-- Name: ProductRecord_ProductRecordId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "ProductRecord_ProductRecordId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2037 (class 0 OID 0)
-- Dependencies: 176
-- Name: ProductRecord_ProductRecordId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "ProductRecord_ProductRecordId_seq" OWNED BY productrecord.productrecordid;


--
-- TOC entry 177 (class 1259 OID 16563)
-- Name: producttype; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE producttype (
    producttypeid integer NOT NULL,
    name text NOT NULL
);


--
-- TOC entry 178 (class 1259 OID 16569)
-- Name: ProductType_TypeId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "ProductType_TypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2038 (class 0 OID 0)
-- Dependencies: 178
-- Name: ProductType_TypeId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "ProductType_TypeId_seq" OWNED BY producttype.producttypeid;


--
-- TOC entry 174 (class 1259 OID 16549)
-- Name: product; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE product (
    productid integer NOT NULL,
    name text NOT NULL,
    producttypeid integer NOT NULL
);


--
-- TOC entry 179 (class 1259 OID 16571)
-- Name: Product_ProductId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "Product_ProductId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2039 (class 0 OID 0)
-- Dependencies: 179
-- Name: Product_ProductId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "Product_ProductId_seq" OWNED BY product.productid;


--
-- TOC entry 172 (class 1259 OID 16541)
-- Name: datasource; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE datasource (
    datasourceid integer NOT NULL,
    name text NOT NULL
);


--
-- TOC entry 173 (class 1259 OID 16547)
-- Name: datasource_datasourceId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "datasource_datasourceId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2040 (class 0 OID 0)
-- Dependencies: 173
-- Name: datasource_datasourceId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "datasource_datasourceId_seq" OWNED BY datasource.datasourceid;


--
-- TOC entry 1903 (class 2604 OID 16573)
-- Name: datasourceid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY datasource ALTER COLUMN datasourceid SET DEFAULT nextval('"datasource_datasourceId_seq"'::regclass);


--
-- TOC entry 1904 (class 2604 OID 16574)
-- Name: productid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY product ALTER COLUMN productid SET DEFAULT nextval('"Product_ProductId_seq"'::regclass);


--
-- TOC entry 1905 (class 2604 OID 16575)
-- Name: productrecordid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord ALTER COLUMN productrecordid SET DEFAULT nextval('"ProductRecord_ProductRecordId_seq"'::regclass);


--
-- TOC entry 1906 (class 2604 OID 16576)
-- Name: producttypeid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY producttype ALTER COLUMN producttypeid SET DEFAULT nextval('"ProductType_TypeId_seq"'::regclass);


--
-- TOC entry 1914 (class 2606 OID 16580)
-- Name: ProductRecord_PK_ProductRecordId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord
    ADD CONSTRAINT "ProductRecord_PK_ProductRecordId" PRIMARY KEY (productrecordid);


--
-- TOC entry 1916 (class 2606 OID 16582)
-- Name: ProductType_PK_ProductTypeId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY producttype
    ADD CONSTRAINT "ProductType_PK_ProductTypeId" PRIMARY KEY (producttypeid);


--
-- TOC entry 1912 (class 2606 OID 16584)
-- Name: Product_PK_ProductId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY product
    ADD CONSTRAINT "Product_PK_ProductId" PRIMARY KEY (productid);


--
-- TOC entry 1908 (class 2606 OID 16578)
-- Name: datasource_PK_datasourceId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY datasource
    ADD CONSTRAINT "datasource_PK_datasourceId" PRIMARY KEY (datasourceid);


--
-- TOC entry 1910 (class 2606 OID 16600)
-- Name: datasource_unique_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY datasource
    ADD CONSTRAINT datasource_unique_name UNIQUE (name);


--
-- TOC entry 1918 (class 2606 OID 16596)
-- Name: producttype_unique_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY producttype
    ADD CONSTRAINT producttype_unique_name UNIQUE (name);


--
-- TOC entry 1920 (class 2606 OID 16585)
-- Name: ProductRecord_FK_ProductId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord
    ADD CONSTRAINT "ProductRecord_FK_ProductId" FOREIGN KEY (productid) REFERENCES product(productid);


--
-- TOC entry 1919 (class 2606 OID 16590)
-- Name: Product_FK_ProductTypeId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY product
    ADD CONSTRAINT "Product_FK_ProductTypeId" FOREIGN KEY (producttypeid) REFERENCES producttype(producttypeid);


-- Completed on 2015-04-27 18:33:07

--
-- PostgreSQL database dump complete
--

