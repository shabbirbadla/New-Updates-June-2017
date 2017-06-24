LPARAMETERS m_regkey,m_regpath,m_regvalue
Installer  =CreateObject("WindowsInstaller.Installer")
m_RegRetpath = ''
m_RegRetpath = Installer.RegistryValue(m_regkey,m_regpath,m_regvalue)
RELEASE Installer
retu m_RegRetpath

