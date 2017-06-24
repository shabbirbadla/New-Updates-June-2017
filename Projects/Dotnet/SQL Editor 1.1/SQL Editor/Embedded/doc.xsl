<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/TR/WD-xsl">
	<xsl:template>
		<xsl:apply-templates/>
	</xsl:template>
	<xsl:template match="/members">

		<HTML>
		<style>
			.mainHeader {font-size:18px; font-weight: bold; line-height:40px; font-family: verdana; background-color: steelblue}
			.subHeader {font-size:12px; font-family: verdana; font-weight: bold;}
			.normal {font-family:verdana; font-size:12px; border:1px black solid;}
		</style>
			<BODY></BODY>
		</HTML>

<xsl:for-each select="member">
	<table width="800" border="0" cellpadding="0" cellspacing="0" class="normal">
			<tr>
				<td colspan="4" class="mainHeader">
					<xsl:value-of select="@name"/>
				</td>
			</tr>
			<tr>
				<td width="120" valign="top" class="subHeader">Summary</td>
				<td colspan="3"><xsl:value-of select="summary"/></td>
			</tr>
			<tr>
				<td></td>
			</tr>
			<tr>
				<td valign="top" class="subHeader">Parameters</td>
			</tr>
<!-- Parameters -->

			<tr>
				<td />
				<td width="200" class="subHeader">Name</td>
				<td colspan="2" class="subHeader">Description</td>
			</tr>
<xsl:for-each select="param">
			<tr>
				<td/>
				<td valign="top"><xsl:value-of select="@name"/></td>
				<td colspan="2"><xsl:value-of select="."/></td>
			</tr>
</xsl:for-each>	
			<tr>
				<td></td>
			</tr>
<!-- /Parameters -->
<!-- Revision -->
			<tr>
				<td valign="top" class="subHeader">Revision</td>
			</tr>
			<tr>
				<td/>
			<td class="subHeader">Author</td>
			<td width="150" class="subHeader">Date</td>
			<td width="280" class="subHeader">Description</td>			</tr>
<xsl:for-each select="revision">
			<tr>
				<td/>
				<td valign="top"><xsl:value-of select="@author"/></td>
				<td align="left" valign="top"><xsl:value-of select="@date"/></td>
				<td valign="top"><xsl:value-of select="."/></td>
			</tr>
</xsl:for-each>
<!-- /Revision -->
	</table>
</xsl:for-each>
	</xsl:template>
	
</xsl:stylesheet>

