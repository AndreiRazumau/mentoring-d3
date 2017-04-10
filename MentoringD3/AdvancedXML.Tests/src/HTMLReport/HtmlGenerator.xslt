<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:user="urn:my-scripts"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt">

  <xsl:output method="html" indent="yes"/>
  <xsl:key name="genre" match="/catalog/book/genre/text()" use="."/>

  <msxsl:script implements-prefix='user' language='CSharp'>
    <![CDATA[
    public string formatDate(string date) {
      return System.DateTime.ParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture).ToString("D");
    }
    
     public string getCurrentDate() {
      return System.DateTime.Now.ToString("D");
    }
    ]]>
  </msxsl:script>

  <xsl:template match="/">
    <xsl:element name="html">
      <xsl:element name="head">
        <xsl:element name="title">
          Текущие фонды по жанрам
        </xsl:element>
      </xsl:element>
      <xsl:element name="body">
        <xsl:element name="div">
          Отчет за  <xsl:value-of select="user:getCurrentDate()"/>
        </xsl:element>
        <xsl:element name="div">
          <xsl:for-each select="/catalog/book/genre/text()[generate-id() = generate-id(key('genre',.)[1])]">
            <xsl:variable name="genre" select="."/>
            <xsl:element name="div">
              <xsl:attribute name="style">margin-top: 20px</xsl:attribute>
              <xsl:element name="table">
                <xsl:attribute name="border">1</xsl:attribute>
                <xsl:element name="caption">
                  <xsl:attribute name="style">font-weight: bold</xsl:attribute>
                  <xsl:value-of select="concat($genre,' (count:',count(//book[genre/text() = $genre]),')')"/>
                </xsl:element>
                <xsl:element name="tr">
                  <xsl:element name="th">
                    Author
                  </xsl:element>
                  <xsl:element name="th">
                    Name
                  </xsl:element>
                  <xsl:element name="th">
                    Publish Date
                  </xsl:element>
                  <xsl:element name="th">
                    Registration Date
                  </xsl:element>
                </xsl:element>
                <xsl:for-each select="//book[genre/text() = $genre]">
                  <xsl:element name="tr">
                    <xsl:element name="td">
                      <xsl:value-of select="author/text()" />
                    </xsl:element>
                    <xsl:element name="td">
                      <xsl:value-of select="title/text()" />
                    </xsl:element>
                    <xsl:element name="td">
                      <xsl:value-of select="user:formatDate(publish_date/text())"/>
                    </xsl:element>
                    <xsl:element name="td">
                      <xsl:value-of select="user:formatDate(registration_date/text())"/>
                    </xsl:element>
                  </xsl:element>
                </xsl:for-each>
              </xsl:element>
            </xsl:element>
          </xsl:for-each>
          <xsl:apply-templates select="node() | @*"/>
        </xsl:element>
      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="text() | @*"/>
</xsl:stylesheet>
