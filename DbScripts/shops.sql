--
-- PostgreSQL database dump
--

-- Dumped from database version 9.4.1
-- Dumped by pg_dump version 9.4.1
-- Started on 2015-08-25 13:18:12

SET statement_timeout = 0;
SET lock_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SET check_function_bodies = false;
SET client_min_messages = warning;

--
-- TOC entry 182 (class 3079 OID 11855)
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- TOC entry 2060 (class 0 OID 0)
-- Dependencies: 182
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: -
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET search_path = public, pg_catalog;

SET default_with_oids = false;

--
-- TOC entry 172 (class 1259 OID 16527)
-- Name: productrecord; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE productrecord (
    productrecordid integer NOT NULL,
    sourceproductid integer NOT NULL,
    price integer NOT NULL,
    rating real,
    "timestamp" timestamp without time zone NOT NULL,
    amountavailable integer,
    description text,
    name text,
    locationid integer NOT NULL,
    externalid text,
    brand text
);


--
-- TOC entry 173 (class 1259 OID 16533)
-- Name: ProductRecord_ProductRecordId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "ProductRecord_ProductRecordId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2061 (class 0 OID 0)
-- Dependencies: 173
-- Name: ProductRecord_ProductRecordId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "ProductRecord_ProductRecordId_seq" OWNED BY productrecord.productrecordid;


--
-- TOC entry 174 (class 1259 OID 16535)
-- Name: producttype; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE producttype (
    producttypeid integer NOT NULL,
    name text NOT NULL
);


--
-- TOC entry 175 (class 1259 OID 16541)
-- Name: ProductType_TypeId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "ProductType_TypeId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2062 (class 0 OID 0)
-- Dependencies: 175
-- Name: ProductType_TypeId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "ProductType_TypeId_seq" OWNED BY producttype.producttypeid;


--
-- TOC entry 176 (class 1259 OID 16543)
-- Name: product; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE product (
    productid integer NOT NULL,
    name text NOT NULL,
    producttypeid integer NOT NULL
);


--
-- TOC entry 177 (class 1259 OID 16549)
-- Name: Product_ProductId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "Product_ProductId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2063 (class 0 OID 0)
-- Dependencies: 177
-- Name: Product_ProductId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "Product_ProductId_seq" OWNED BY product.productid;


--
-- TOC entry 178 (class 1259 OID 16551)
-- Name: datasource; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE datasource (
    datasourceid integer NOT NULL,
    name text NOT NULL
);


--
-- TOC entry 179 (class 1259 OID 16557)
-- Name: datasource_datasourceId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE "datasource_datasourceId_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 2064 (class 0 OID 0)
-- Dependencies: 179
-- Name: datasource_datasourceId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE "datasource_datasourceId_seq" OWNED BY datasource.datasourceid;


--
-- TOC entry 180 (class 1259 OID 16559)
-- Name: location; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE location (
    locationid integer NOT NULL,
    name text NOT NULL
);


--
-- TOC entry 181 (class 1259 OID 16565)
-- Name: sourceproduct; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE sourceproduct (
    sourceproductid integer NOT NULL,
    datasourceid integer NOT NULL,
    key text NOT NULL,
    name text NOT NULL,
    originalname text NOT NULL,
    productid integer NOT NULL
);


--
-- TOC entry 1916 (class 2604 OID 16571)
-- Name: datasourceid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY datasource ALTER COLUMN datasourceid SET DEFAULT nextval('"datasource_datasourceId_seq"'::regclass);


--
-- TOC entry 1915 (class 2604 OID 16572)
-- Name: productid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY product ALTER COLUMN productid SET DEFAULT nextval('"Product_ProductId_seq"'::regclass);


--
-- TOC entry 1913 (class 2604 OID 16573)
-- Name: productrecordid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord ALTER COLUMN productrecordid SET DEFAULT nextval('"ProductRecord_ProductRecordId_seq"'::regclass);


--
-- TOC entry 1914 (class 2604 OID 16574)
-- Name: producttypeid; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY producttype ALTER COLUMN producttypeid SET DEFAULT nextval('"ProductType_TypeId_seq"'::regclass);


--
-- TOC entry 1918 (class 2606 OID 16576)
-- Name: ProductRecord_PK_ProductRecordId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord
    ADD CONSTRAINT "ProductRecord_PK_ProductRecordId" PRIMARY KEY (productrecordid);


--
-- TOC entry 1922 (class 2606 OID 16578)
-- Name: ProductType_PK_ProductTypeId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY producttype
    ADD CONSTRAINT "ProductType_PK_ProductTypeId" PRIMARY KEY (producttypeid);


--
-- TOC entry 1926 (class 2606 OID 16580)
-- Name: Product_PK_ProductId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY product
    ADD CONSTRAINT "Product_PK_ProductId" PRIMARY KEY (productid);


--
-- TOC entry 1928 (class 2606 OID 16582)
-- Name: datasource_PK_datasourceId; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY datasource
    ADD CONSTRAINT "datasource_PK_datasourceId" PRIMARY KEY (datasourceid);


--
-- TOC entry 1930 (class 2606 OID 16584)
-- Name: datasource_unique_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY datasource
    ADD CONSTRAINT datasource_unique_name UNIQUE (name);


--
-- TOC entry 1932 (class 2606 OID 16586)
-- Name: location_pk_locationid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY location
    ADD CONSTRAINT location_pk_locationid PRIMARY KEY (locationid);


--
-- TOC entry 1934 (class 2606 OID 16588)
-- Name: location_unique_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY location
    ADD CONSTRAINT location_unique_name UNIQUE (name);


--
-- TOC entry 1924 (class 2606 OID 16590)
-- Name: producttype_unique_name; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY producttype
    ADD CONSTRAINT producttype_unique_name UNIQUE (name);


--
-- TOC entry 1937 (class 2606 OID 16592)
-- Name: sourceproduct_pk_sourceproductid; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY sourceproduct
    ADD CONSTRAINT sourceproduct_pk_sourceproductid PRIMARY KEY (sourceproductid);


--
-- TOC entry 1939 (class 2606 OID 16594)
-- Name: sourceproduct_unique_datasourceid_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY sourceproduct
    ADD CONSTRAINT sourceproduct_unique_datasourceid_key UNIQUE (datasourceid, key);


--
-- TOC entry 1919 (class 1259 OID 16595)
-- Name: fki_productrecord_fk_locationid; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_productrecord_fk_locationid ON productrecord USING btree (locationid);


--
-- TOC entry 1920 (class 1259 OID 16596)
-- Name: fki_productrecord_fk_sourceproductid; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_productrecord_fk_sourceproductid ON productrecord USING btree (sourceproductid);


--
-- TOC entry 1935 (class 1259 OID 16622)
-- Name: fki_sourceproduct_fk_productid; Type: INDEX; Schema: public; Owner: -
--

CREATE INDEX fki_sourceproduct_fk_productid ON sourceproduct USING btree (productid);


--
-- TOC entry 1942 (class 2606 OID 16597)
-- Name: Product_FK_ProductTypeId; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY product
    ADD CONSTRAINT "Product_FK_ProductTypeId" FOREIGN KEY (producttypeid) REFERENCES producttype(producttypeid);


--
-- TOC entry 1940 (class 2606 OID 16602)
-- Name: productrecord_fk_locationid; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord
    ADD CONSTRAINT productrecord_fk_locationid FOREIGN KEY (locationid) REFERENCES location(locationid);


--
-- TOC entry 1941 (class 2606 OID 16607)
-- Name: productrecord_fk_sourceproductid; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY productrecord
    ADD CONSTRAINT productrecord_fk_sourceproductid FOREIGN KEY (sourceproductid) REFERENCES sourceproduct(sourceproductid);


--
-- TOC entry 1943 (class 2606 OID 16612)
-- Name: sourceproduct_fk_datasourceid; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY sourceproduct
    ADD CONSTRAINT sourceproduct_fk_datasourceid FOREIGN KEY (datasourceid) REFERENCES datasource(datasourceid);


--
-- TOC entry 1944 (class 2606 OID 16617)
-- Name: sourceproduct_fk_productid; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY sourceproduct
    ADD CONSTRAINT sourceproduct_fk_productid FOREIGN KEY (productid) REFERENCES product(productid);


-- Completed on 2015-08-25 13:18:12

--
-- PostgreSQL database dump complete
--

