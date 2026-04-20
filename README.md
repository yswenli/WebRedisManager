<div align="center">

# 🎯 WebRedisManager

**让 Redis 管理像喝水一样简单**

[![GitHub release](https://img.shields.io/github/release/yswenli/webredismanager.svg?style=flat-square)](https://github.com/yswenli/WebRedisManager/releases)
[![GitHub stars](https://img.shields.io/github/stars/yswenli/WebRedisManager.svg?style=flat-square)](https://github.com/yswenli/WebRedisManager/stargazers)
[![License](https://img.shields.io/github/license/yswenli/WebRedisManager.svg?style=flat-square)](https://github.com/yswenli/WebRedisManager/blob/master/LICENSE)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4.svg?style=flat-square)](https://dotnet.microsoft.com/download/dotnet/8.0)

[🇨🇳 中文文档](README.md) | [🇺🇸 English](README_en.md)

---

还在用命令行一条条敲 Redis 命令？🤔

还在为记不住 Redis 命令参数而头疼？🤯

还在为了查看一个 key 的值折腾半天？😫

**WebRedisManager 来拯救你！** 🚀

</div>

---

## ✨ 它能做什么？

想象一下，所有的 Redis 操作都能像操作文件夹一样简单直观：

| 🎁 功能 | 💡 就像... |
|--------|-----------|
| 数据浏览 | 像浏览电脑文件夹一样查看 Redis 里的所有数据 |
| 键值编辑 | 像编辑文本文件一样修改 Redis 数据 |
| 批量操作 | 一键选中、批量删除，告别手动一个个敲命令 |
| 实时监控 | 像 Task Manager 看电脑状态一样看 Redis 状态 |
| 集群管理 | 图形化管理集群节点，不用背复杂的集群命令 |
| 命令终端 | 想敲命令？这里也有，还带智能提示哦 |

### 支持的数据类型

```
📦 String   → 存字符串、数字、JSON
📦 Hash     → 存对象，像一个小字典
📦 List     → 存列表，像队列
📦 Set      → 存集合，自动去重
📦 ZSet     → 存排行榜，自带排序
```

---

## 🚀 三分钟上手

### 第一步：准备工作

你需要准备：
- ✅ 一台电脑（Windows/Linux/macOS 都可以）
- ✅ 安装 [.NET 8.0 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0)
- ✅ 一个运行中的 Redis 服务

### 第二步：下载运行

#### 🎮 方式一：下载即用（推荐新手）

1. 点击 [这里](https://github.com/yswenli/WebRedisManager/releases) 下载最新版本
2. 解压，双击 `WebRedisManager.exe`（Windows）或运行 `dotnet WebRedisManager.dll`（Linux/Mac）
3. 浏览器自动打开 `http://localhost:16379`

就这么简单！🎉

#### 🐳 方式二：Docker 一键部署

```bash
docker run -d -p 16379:80 --name redis-manager yswenli/webredis-manager
```

#### 🔧 方式三：从源码编译

```bash
git clone https://github.com/yswenli/WebRedisManager.git
cd WebRedisManager
dotnet run --project SAEA.WebRedisManager
```

### 第三步：连接 Redis

打开浏览器后：

1. 点击 **「添加连接」**
2. 填写 Redis 地址和端口（本地的就是 `127.0.0.1:6379`）
3. 如果有密码，填上密码
4. 点击 **「连接」**

搞定！开始探索你的 Redis 数据吧！🎊

---

## 🎨 界面预览

### 👋 登录界面 - 干净清爽
<img src="https://raw.githubusercontent.com/yswenli/WebRedisManager/master/webredismanager001.png" width="650">

### 🏠 主界面 - 一目了然
<img src="https://raw.githubusercontent.com/yswenli/WebRedisManager/master/webredismanager002.png" width="650">
> 左侧是数据库和键的树形结构，右侧是数据内容，想看什么点什么

### 📊 数据浏览 - 树形结构
<img src="https://raw.githubusercontent.com/yswenli/WebRedisManager/master/webredismanager003.png" width="650">
> 就像资源管理器一样，层层展开，清清楚楚

### ✏️ 编辑数据 - 所见即所得
<img src="https://raw.githubusercontent.com/yswenli/WebRedisManager/master/webredismanager004.png" width="650">
> 直接编辑，支持 JSON 格式化，改完保存即可

### 🌐 集群管理 - 可视化操作
<img src="https://raw.githubusercontent.com/yswenli/WebRedisManager/master/webredismanager005.png" width="650">
> 集群节点、槽位分配，全部图形化展示

### 📈 监控图表 - 实时状态
<img src="https://raw.githubusercontent.com/yswenli/WebRedisManager/master/webredismanager006.png" width="650">
> 内存使用、连接数、命令统计，一目了然

### 💻 控制台 - 命令行爱好者的福音
<img src="https://raw.githubusercontent.com/yswenli/WebRedisManager/master/webredismanager007.png" width="650">
> 内置 Redis 命令终端，想敲命令？这里也能满足你

---

## 🛠️ 技术栈

本项目基于 **SAEA** 系列高性能组件构建，这是一套国人自主研发的通信框架。

### SAEA 技术家族

```
┌─────────────────────────────────────────────────────────────┐
│                    SAEA 技术栈全景图                          │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│   🔌 SAEA.Sockets (IOCP 底层通信引擎)                        │
│      │                                                      │
│      ├── 📡 SAEA.Http        → HTTP 服务器                   │
│      │                                                      │
│      ├── 🌐 SAEA.MVC         → Web MVC 框架                  │
│      │                                                      │
│      ├── 🔴 SAEA.RedisSocket → Redis 客户端                  │
│      │                                                      │
│      ├── 💬 SAEA.WebSocket   → WebSocket 服务                │
│      │                                                      │
│      └── 📦 SAEA.Common      → 公共工具库                    │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

### 🔌 SAEA.Sockets - 高性能 IOCP 通信引擎

> **一句话理解**：就像给你的程序装了个"涡轮增压器"，让网络通信飞起来！

**什么是 IOCP？**

IOCP（I/O Completion Port）是 Windows 平台最高效的异步通信模型。打个比方：

| 传统方式 | IOCP 方式 |
|---------|----------|
| 每个客户来吃饭，需要一个服务员专门服务 | 一个服务员可以同时照顾很多桌子 |
| 100 个客户 = 100 个服务员 = 100 个线程 | 100 个客户 = 1 个服务员 = 极少线程 |
| 资源消耗大，容易卡顿 | 资源利用率高，流畅高效 |

**SAEA.Sockets 核心特性**

| 特性 | 说明 | 带给你的好处 |
|------|------|-------------|
| **BufferPool 内存池** | 预分配内存，重复利用 | 减少 GC 压力，避免内存抖动 |
| **Session 会话管理** | 自动管理连接状态 | 断线重连、超时处理全自动 |
| **异步事件驱动** | 完成端口通知机制 | 单线程处理万级并发 |
| **协议编解码** | 内置多种编码器 | TCP 粘包分包自动处理 |

**性能对比**

```
传统 Socket：    5000 连接 → 延迟 5ms  → CPU 60%
SAEA.Sockets：   5000 连接 → 延迟 1ms  → CPU 15%

传统 Socket：    10000 连接 → 延迟 10ms → CPU 80%
SAEA.Sockets：   10000 连接 → 延迟 2ms  → CPU 25%
```

👉 [了解更多](https://www.nuget.org/packages/SAEA.Sockets)

---

### 🌐 SAEA.MVC - 轻量级自宿主 Web 框架

> **一句话理解**：像 ASP.NET Core 一样好用，但更轻更快更简单！

**为什么选择 SAEA.MVC？**

| 对比项 | ASP.NET Core | SAEA.MVC |
|--------|--------------|----------|
| 启动时间 | 500-1000ms | **50ms** |
| 内存占用 | 80-150MB | **20MB** |
| 部署方式 | 需要 Kestrel/IIS | **自宿主，单文件** |
| 学习曲线 | 较陡 | **平滑，像老版 MVC** |
| 依赖复杂度 | 较多 | **极少** |

**核心功能一览**

```
🎯 Controller/Action 路由     → 熟悉的 MVC 开发体验
🔒 AOP 过滤器                  → 权限控制、日志拦截
💾 OutputCache 输出缓存       → 方法级缓存，响应提速 50 倍
📡 SSE 服务器推送             → 实时消息推送，无需 WebSocket
📁 静态文件服务               → 自动缓存、大文件分块传输
```

**代码示例：5 行代码启动 Web 服务**

```csharp
var config = SAEAMvcApplicationConfigBuilder.Read();
var app = new SAEAMvcApplication(config);
app.SetDefault("home", "index");
app.Start();
// 就这么简单！浏览器访问 localhost:28080
```

👉 [了解更多](https://www.nuget.org/packages/SAEA.MVC)

---

### 🔴 SAEA.RedisSocket - 高性能 Redis 客户端

> **一句话理解**：Redis 操作的"瑞士军刀"，又快又全能！

**核心优势**

| 特性 | 说明 | 实际用途 |
|------|------|---------|
| **完整数据类型** | String/Hash/List/Set/ZSet/GEO | 覆盖所有 Redis 数据结构 |
| **Redis Cluster** | 自动重定向、槽位计算 | 集群模式无缝支持 |
| **分布式锁** | SETNX + Lua 脚本 | 秒杀、防并发冲突 |
| **Pipeline 批量** | 命令打包发送 | 减少 90% 网络往返 |
| **Stream 消息** | Producer/Consumer | 分布式消息队列 |
| **Pub/Sub** | 发布订阅 | 实时消息推送 |

**性能数据**

| 操作 | QPS | 延迟 |
|------|-----|------|
| SET | 120,000 | 0.8ms |
| GET | 150,000 | 0.6ms |
| HSET | 100,000 | 1.0ms |
| Pipeline(100命令) | 50,000 batch | 2ms |

**代码示例：3 行代码操作 Redis**

```csharp
var client = new RedisClient("server=127.0.0.1:6379");
client.Connect();
var db = client.GetDataBase();
// 开始操作：db.Set("key", "value")、db.Get("key")...
```

👉 [了解更多](https://www.nuget.org/packages/SAEA.RedisSocket)

---

### 💬 SAEA.WebSocket - 实时双向通信

> **一句话理解**：让服务器和浏览器能像打电话一样实时对话！

用于本项目的数据实时推送：
- Redis 监控数据实时更新
- 控制台命令实时响应
- 集群状态实时同步

---

### 📦 SAEA.Common - 公共工具库

提供常用工具：
- 序列化/反序列化
- 日志记录
- 配置管理
- 时间/字符串处理

---

### 其他技术组件

| 组件 | 用途 |
|------|------|
| [.NET 8.0](https://dotnet.microsoft.com/) | 运行时平台，跨平台支持 |
| [Layui](https://layui.dev/) | 前端 UI 框架，经典美观 |
| [ECharts](https://echarts.apache.org/) | 数据可视化图表 |

---

### 默认端口

| 端口 | 干嘛用的 |
|------|---------|
| `16379` | Web 管理界面 (SAEA.MVC) |
| `26379` | WebSocket 实时推送 |

---

### 🏗️ 项目架构

```
浏览器请求 ──► SAEA.MVC (端口 16379)
                     │
                     ▼
              Controllers 处理业务逻辑
                     │
                     ▼
              Services 调用 Redis 操作
                     │
                     ▼
              SAEA.RedisSocket 连接 Redis
                     │
                     ▼
                  Redis 服务器

WebSocket ──► SAEA.WebSocket (端口 26379)
                     │
                     ▼
              实时推送监控数据/命令响应
```

---

## ❓ 常见问题

<details>
<summary><b>🔴 Q: 双击运行后闪退怎么办？</b></summary>

别慌！大概率是没装 .NET 8.0 Runtime。

1. 去 [微软官网](https://dotnet.microsoft.com/download/dotnet/8.0) 下载安装
2. 或者用命令行运行 `dotnet WebRedisManager.dll`，会显示具体错误信息
</details>

<details>
<summary><b>🔴 Q: 连不上 Redis 服务器？</b></summary>

按这个清单检查一下：

- [ ] Redis 服务启动了吗？
- [ ] 端口对吗？（默认 6379）
- [ ] 防火墙放行了吗？
- [ ] Redis 配置的 `bind` 地址对吗？
- [ ] 密码填对了吗？
</details>

<details>
<summary><b>🔴 Q: 如何连接 Redis 集群？</b></summary>

超级简单！只需要填写集群中任意一个节点的地址，工具会自动发现其他节点。

集群相关的槽位分配、主从关系都会自动展示出来。
</details>

<details>
<summary><b>🔴 Q: 支持哪些 Redis 版本？</b></summary>

支持 Redis 3.0 及以上版本，包括：
- 单机模式
- 哨兵模式  
- 集群模式（Cluster）
</details>

---

## 🤝 想参与开发？

欢迎！我们非常需要你的帮助：

```bash
# 1. Fork 仓库
# 2. 克隆你的 Fork
git clone https://github.com/你的用户名/WebRedisManager.git

# 3. 创建功能分支
git checkout -b feature/超酷的功能

# 4. 写代码、测试
dotnet run --project SAEA.WebRedisManager

# 5. 提交
git commit -m "Add: 添加了超酷的功能"

# 6. 推送
git push origin feature/超酷的功能

# 7. 创建 Pull Request
```

---

## 📞 联系我们

遇到问题？有建议？想聊天？

| 渠道 | 地址 |
|------|------|
| 📝 博客 | [点击访问](https://www.cnblogs.com/yswenli/p/9460527.html) |
| 💬 QQ群 | **788260487** |
| 🐛 提Bug | [Issues](https://github.com/yswenli/WebRedisManager/issues) |

---

## 📜 开源协议

本项目采用 [MIT License](LICENSE) 开源协议。

简单说就是：**随便用，但请保留原作者信息**。

---

<div align="center">

---

**如果这个工具帮到了你，请给一个 ⭐️ Star**

这是对开发者最大的鼓励！🙏

Made with ❤️ by [yswenli](https://github.com/yswenli)

</div>